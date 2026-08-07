using System;
using System.Collections.Generic;
using ExtensibleSaveFormat;
using UnityEngine;

namespace ThighPhysicsController;

public sealed class ThighParams
{
    public const string DataKey = "codex.koikatumanager.thighphysicscontroller";

    // v54 was used by the archived/broken 0.9.0 collider build; keep it skipped.
    public const int DataVersion = 55;

    public bool Enabled = true;

    public bool GamePhysics;

    public float Gravity;

    public float Weight = 0.5f;

    /// <summary>Dance/motion response multiplier, 0..5 (UI label: "Dance response").</summary>
    public float MotionGain = 1f;

    /// <summary>Spring jitter/oscillation frequency (0..2.5, 1 = default).</summary>
    public float JitterFreq = 1f;

    /// <summary>Spring motion-response smoothing (0.05..0.5; lower = smoother).</summary>
    public float MotionSmooth = 0.25f;

    public ThighBoneParams Thigh00 = new ThighBoneParams();

    public ThighBoneAmounts Bones = new ThighBoneAmounts();

    /// <summary>Per-bone settings used only by chain mode (kept separate from spring Bones).</summary>
    public ThighBoneAmounts ChainBones = new ThighBoneAmounts();

    public ChainParams Chain = new ChainParams();

    public static ThighParams CreateDefault()
    {
        ThighParams p = new ThighParams();
        p.Enabled = true;
        p.GamePhysics = false;
        p.Gravity = 0.05f;
        p.Weight = 0.7f;
        p.MotionGain = 1f;
        p.JitterFreq = 1f;
        p.MotionSmooth = 0.25f;
        p.Thigh00.IsRotationCalc = true;
        p.Thigh00.Damping = 0.12f;
        p.Thigh00.Elasticity = 0.02f;
        p.Thigh00.Stiffness = 0.08f;
        p.Thigh00.Inert = 0.30f;
        p.Thigh00.CollisionRadius = 0.03f;
        p.Thigh00.LeverLength = 0f;
        p.Thigh00.ReflectSpeed = 1f;
        p.Thigh00.SwayAmplitude = 0.008f;
        p.Thigh00.DriveGain = 0.5f;
        p.Thigh00.Spring = 60f;
        p.Thigh00.PendulumDamping = 0.55f;
        p.Chain = new ChainParams
        {
            Weight = 0.7f,
            Gravity = 0.05f,
            Damping = 0.30f,
            Elasticity = 0.25f,
            Stiffness = 0.9f,
            Inert = 0.40f,
            JitterFreq = 1f,
        };
        p.Bones.SetDefaults();
        p.ChainBones.SetChainDefaults();
        return p;
    }

    public static ThighParams CreatePartDefaults(FleshPartId part)
    {
        ThighParams p = CreateDefault();
        float scale = part == FleshPartId.Arm ? 0.6f : part == FleshPartId.Belly ? 0.25f : 1f;
        if (scale < 1f)
        {
            for (int i = 0; i < 4; i++)
            {
                p.Bones.Get(i).Amp *= scale;
                p.ChainBones.Get(i).Amp *= scale;
            }
        }
        return p;
    }

    public void WriteData(PluginData data)
    {
        WritePart(data.data, "", this);
    }

    public void ReadData(PluginData data)
    {
        int version = 0;
        if (data.data.ContainsKey("v"))
        {
            version = Convert.ToInt32(data.data["v"]);
        }
        ReadPart(data.data, "", this, version);
    }

    public static void WritePart(Dictionary<string, object> data, string prefix, ThighParams p)
    {
        if (prefix.Length == 0)
        {
            data["v"] = DataVersion;
        }
        data[prefix + "enabled"] = p.Enabled;
        data[prefix + "gp"] = p.GamePhysics;
        data[prefix + "gravity"] = p.Gravity;
        data[prefix + "weight"] = p.Weight;
        data[prefix + "mg"] = p.MotionGain;
        data[prefix + "jf"] = p.JitterFreq;
        data[prefix + "ms"] = p.MotionSmooth;
        data[prefix + "c_w"] = p.Chain.Weight;
        data[prefix + "c_g"] = p.Chain.Gravity;
        data[prefix + "c_d"] = p.Chain.Damping;
        data[prefix + "c_e"] = p.Chain.Elasticity;
        data[prefix + "c_s"] = p.Chain.Stiffness;
        data[prefix + "c_i"] = p.Chain.Inert;
        data[prefix + "c_jf"] = p.Chain.JitterFreq;
        WriteBone(data, prefix + "t00", p.Thigh00);
        WriteBoneAmounts(data, prefix, p.Bones);
        WriteChainBoneAmounts(data, prefix, p.ChainBones);
    }

    public static void ReadPart(Dictionary<string, object> data, string prefix, ThighParams p, int version)
    {
        if (data.ContainsKey(prefix + "enabled"))
        {
            p.Enabled = Convert.ToBoolean(data[prefix + "enabled"]);
        }
        if (data.ContainsKey(prefix + "gp"))
        {
            p.GamePhysics = Convert.ToBoolean(data[prefix + "gp"]);
        }
        if (data.ContainsKey(prefix + "gravity"))
        {
            p.Gravity = Convert.ToSingle(data[prefix + "gravity"]);
        }
        if (data.ContainsKey(prefix + "weight"))
        {
            p.Weight = Convert.ToSingle(data[prefix + "weight"]);
        }
        if (data.ContainsKey(prefix + "mg"))
        {
            p.MotionGain = Mathf.Clamp(Convert.ToSingle(data[prefix + "mg"]), 0f, 5f);
        }
        if (data.ContainsKey(prefix + "jf"))
        {
            p.JitterFreq = Mathf.Clamp(Convert.ToSingle(data[prefix + "jf"]), 0f, 2.5f);
        }
        if (data.ContainsKey(prefix + "ms"))
        {
            p.MotionSmooth = Mathf.Clamp(Convert.ToSingle(data[prefix + "ms"]), 0.05f, 0.5f);
        }
        if (data.ContainsKey(prefix + "c_w"))
        {
            p.Chain.Weight = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_w"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "c_g"))
        {
            p.Chain.Gravity = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_g"]), -0.2f, 0.2f);
        }
        if (data.ContainsKey(prefix + "c_d"))
        {
            p.Chain.Damping = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_d"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "c_e"))
        {
            p.Chain.Elasticity = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_e"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "c_s"))
        {
            p.Chain.Stiffness = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_s"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "c_i"))
        {
            p.Chain.Inert = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_i"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "c_jf"))
        {
            p.Chain.JitterFreq = Mathf.Clamp(Convert.ToSingle(data[prefix + "c_jf"]), 0f, 2.5f);
        }
        ReadBone(data, prefix + "t00", p.Thigh00);
        if (version < 53)
        {
            // v52 and earlier: KneeF was index 3 (b3), Leg02 was index 4 (b4).
            // KneeF is removed; Leg02 is now index 3 (b3). Migrate from b4.
            ReadLegacyBoneAmounts(data, prefix, p.Bones);
            ReadLegacyChainBoneAmounts(data, prefix, p.ChainBones);
        }
        else
        {
            ReadBoneAmounts(data, prefix, p.Bones);
            ReadChainBoneAmounts(data, prefix, p.ChainBones);
        }
        if (version < 51)
        {
            // Migration for cards saved before 0.5.0: force the new default feel.
            p.Weight = 0.8f;
            p.Thigh00.Damping = 0.03f;
            p.Thigh00.Elasticity = 0.02f;
        }
    }

    private static void WriteBone(Dictionary<string, object> data, string prefix, ThighBoneParams bone)
    {
        data[prefix + "_rot"] = bone.IsRotationCalc;
        data[prefix + "_damp"] = bone.Damping;
        data[prefix + "_elas"] = bone.Elasticity;
        data[prefix + "_stif"] = bone.Stiffness;
        data[prefix + "_inert"] = bone.Inert;
        data[prefix + "_rad"] = bone.CollisionRadius;
        data[prefix + "_lever"] = bone.LeverLength;
        data[prefix + "_speed"] = bone.ReflectSpeed;
        data[prefix + "_sway"] = bone.SwayAmplitude;
        data[prefix + "_drive"] = bone.DriveGain;
        data[prefix + "_spring"] = bone.Spring;
        data[prefix + "_pdamp"] = bone.PendulumDamping;
    }

    private static void ReadBone(Dictionary<string, object> data, string prefix, ThighBoneParams bone)
    {
        if (data.ContainsKey(prefix + "_rot"))
        {
            bone.IsRotationCalc = Convert.ToBoolean(data[prefix + "_rot"]);
        }
        if (data.ContainsKey(prefix + "_damp"))
        {
            bone.Damping = Convert.ToSingle(data[prefix + "_damp"]);
        }
        if (data.ContainsKey(prefix + "_elas"))
        {
            bone.Elasticity = Convert.ToSingle(data[prefix + "_elas"]);
        }
        if (data.ContainsKey(prefix + "_stif"))
        {
            bone.Stiffness = Convert.ToSingle(data[prefix + "_stif"]);
        }
        if (data.ContainsKey(prefix + "_inert"))
        {
            bone.Inert = Convert.ToSingle(data[prefix + "_inert"]);
        }
        if (data.ContainsKey(prefix + "_rad"))
        {
            bone.CollisionRadius = Convert.ToSingle(data[prefix + "_rad"]);
        }
        if (data.ContainsKey(prefix + "_lever"))
        {
            bone.LeverLength = Convert.ToSingle(data[prefix + "_lever"]);
        }
        if (data.ContainsKey(prefix + "_speed"))
        {
            bone.ReflectSpeed = Convert.ToSingle(data[prefix + "_speed"]);
        }
        if (data.ContainsKey(prefix + "_sway"))
        {
            bone.SwayAmplitude = Mathf.Clamp(Convert.ToSingle(data[prefix + "_sway"]), 0f, 0.02f);
        }
        if (data.ContainsKey(prefix + "_drive"))
        {
            bone.DriveGain = Mathf.Clamp(Convert.ToSingle(data[prefix + "_drive"]), 0f, 5f);
        }
        if (data.ContainsKey(prefix + "_spring"))
        {
            bone.Spring = Mathf.Clamp(Convert.ToSingle(data[prefix + "_spring"]), 1f, 300f);
        }
        if (data.ContainsKey(prefix + "_pdamp"))
        {
            bone.PendulumDamping = Mathf.Clamp(Convert.ToSingle(data[prefix + "_pdamp"]), 0f, 1f);
        }
    }

    private static void WriteBoneAmounts(Dictionary<string, object> data, string prefix, ThighBoneAmounts bones)
    {
        for (int i = 0; i < 4; i++)
        {
            PerBoneAmount amount = bones.Get(i);
            data[prefix + "b" + i + "_en"] = amount.Enabled;
            data[prefix + "b" + i + "_amp"] = amount.Amp;
            data[prefix + "b" + i + "_ax"] = amount.AxisX;
            data[prefix + "b" + i + "_ay"] = amount.AxisY;
            data[prefix + "b" + i + "_az"] = amount.AxisZ;
            data[prefix + "b" + i + "_rot"] = amount.RotAmp;
            data[prefix + "b" + i + "_rc"] = amount.RotCalc;
        }
    }

    private static void ReadBoneAmounts(Dictionary<string, object> data, string prefix, ThighBoneAmounts bones)
    {
        for (int i = 0; i < 4; i++)
        {
            PerBoneAmount amount = bones.Get(i);
            if (data.ContainsKey(prefix + "b" + i + "_en"))
            {
                amount.Enabled = Convert.ToBoolean(data[prefix + "b" + i + "_en"]);
            }
            if (data.ContainsKey(prefix + "b" + i + "_amp"))
            {
                amount.Amp = Mathf.Clamp(Convert.ToSingle(data[prefix + "b" + i + "_amp"]), 0f, 2f);
            }
            if (data.ContainsKey(prefix + "b" + i + "_ax"))
            {
                amount.AxisX = Mathf.Clamp(Convert.ToSingle(data[prefix + "b" + i + "_ax"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "b" + i + "_ay"))
            {
                amount.AxisY = Mathf.Clamp(Convert.ToSingle(data[prefix + "b" + i + "_ay"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "b" + i + "_az"))
            {
                amount.AxisZ = Mathf.Clamp(Convert.ToSingle(data[prefix + "b" + i + "_az"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "b" + i + "_rot"))
            {
                amount.RotAmp = Mathf.Clamp(Convert.ToSingle(data[prefix + "b" + i + "_rot"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "b" + i + "_rc"))
            {
                amount.RotCalc = Convert.ToBoolean(data[prefix + "b" + i + "_rc"]);
            }
        }
    }

    private static void ReadLegacyBoneAmounts(Dictionary<string, object> data, string prefix, ThighBoneAmounts bones)
    {
        // Old layout: b0..b2 same, b3 = KneeF (dropped), b4 = Leg02.
        ReadBoneAmounts(data, prefix, bones);
        PerBoneAmount leg = bones.Get(3);
        if (data.ContainsKey(prefix + "b4_en"))
        {
            leg.Enabled = Convert.ToBoolean(data[prefix + "b4_en"]);
        }
        if (data.ContainsKey(prefix + "b4_amp"))
        {
            leg.Amp = Mathf.Clamp(Convert.ToSingle(data[prefix + "b4_amp"]), 0f, 2f);
        }
        if (data.ContainsKey(prefix + "b4_ax"))
        {
            leg.AxisX = Mathf.Clamp(Convert.ToSingle(data[prefix + "b4_ax"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "b4_ay"))
        {
            leg.AxisY = Mathf.Clamp(Convert.ToSingle(data[prefix + "b4_ay"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "b4_az"))
        {
            leg.AxisZ = Mathf.Clamp(Convert.ToSingle(data[prefix + "b4_az"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "b4_rot"))
        {
            leg.RotAmp = Mathf.Clamp(Convert.ToSingle(data[prefix + "b4_rot"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "b4_rc"))
        {
            leg.RotCalc = Convert.ToBoolean(data[prefix + "b4_rc"]);
        }
    }

    private static void WriteChainBoneAmounts(Dictionary<string, object> data, string prefix, ThighBoneAmounts bones)
    {
        for (int i = 0; i < 4; i++)
        {
            PerBoneAmount amount = bones.Get(i);
            data[prefix + "cb" + i + "_en"] = amount.Enabled;
            data[prefix + "cb" + i + "_amp"] = amount.Amp;
            data[prefix + "cb" + i + "_ax"] = amount.AxisX;
            data[prefix + "cb" + i + "_ay"] = amount.AxisY;
            data[prefix + "cb" + i + "_az"] = amount.AxisZ;
            data[prefix + "cb" + i + "_rc"] = amount.RotCalc;
        }
    }

    private static void ReadChainBoneAmounts(Dictionary<string, object> data, string prefix, ThighBoneAmounts bones)
    {
        for (int i = 0; i < 4; i++)
        {
            PerBoneAmount amount = bones.Get(i);
            if (data.ContainsKey(prefix + "cb" + i + "_en"))
            {
                amount.Enabled = Convert.ToBoolean(data[prefix + "cb" + i + "_en"]);
            }
            if (data.ContainsKey(prefix + "cb" + i + "_amp"))
            {
                amount.Amp = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb" + i + "_amp"]), 0f, 2f);
            }
            if (data.ContainsKey(prefix + "cb" + i + "_ax"))
            {
                amount.AxisX = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb" + i + "_ax"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "cb" + i + "_ay"))
            {
                amount.AxisY = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb" + i + "_ay"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "cb" + i + "_az"))
            {
                amount.AxisZ = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb" + i + "_az"]), 0f, 1f);
            }
            if (data.ContainsKey(prefix + "cb" + i + "_rc"))
            {
                amount.RotCalc = Convert.ToBoolean(data[prefix + "cb" + i + "_rc"]);
            }
        }
    }

    private static void ReadLegacyChainBoneAmounts(Dictionary<string, object> data, string prefix, ThighBoneAmounts bones)
    {
        ReadChainBoneAmounts(data, prefix, bones);
        PerBoneAmount leg = bones.Get(3);
        if (data.ContainsKey(prefix + "cb4_en"))
        {
            leg.Enabled = Convert.ToBoolean(data[prefix + "cb4_en"]);
        }
        if (data.ContainsKey(prefix + "cb4_amp"))
        {
            leg.Amp = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb4_amp"]), 0f, 2f);
        }
        if (data.ContainsKey(prefix + "cb4_ax"))
        {
            leg.AxisX = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb4_ax"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "cb4_ay"))
        {
            leg.AxisY = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb4_ay"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "cb4_az"))
        {
            leg.AxisZ = Mathf.Clamp(Convert.ToSingle(data[prefix + "cb4_az"]), 0f, 1f);
        }
        if (data.ContainsKey(prefix + "cb4_rc"))
        {
            leg.RotCalc = Convert.ToBoolean(data[prefix + "cb4_rc"]);
        }
    }
}
