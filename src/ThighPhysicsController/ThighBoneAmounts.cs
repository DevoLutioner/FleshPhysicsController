using UnityEngine;

namespace ThighPhysicsController;

public sealed class ThighBoneAmounts
{
    public readonly PerBoneAmount Thigh01 = new PerBoneAmount();
    public readonly PerBoneAmount Thigh02 = new PerBoneAmount();
    public readonly PerBoneAmount Thigh03 = new PerBoneAmount();
    public readonly PerBoneAmount Leg02 = new PerBoneAmount();

    public PerBoneAmount Get(int index)
    {
        return index switch
        {
            0 => Thigh01,
            1 => Thigh02,
            2 => Thigh03,
            _ => Leg02,
        };
    }

    public float GetAmp(int index)
    {
        PerBoneAmount amount = Get(index);
        return amount.Enabled ? amount.Amp : 0f;
    }

    public Vector3 GetAxis(int index)
    {
        PerBoneAmount amount = Get(index);
        return new Vector3(amount.AxisX, amount.AxisY, amount.AxisZ);
    }

    public float GetRotAmp(int index)
    {
        return Get(index).RotAmp;
    }

    public bool GetRotCalc(int index)
    {
        return Get(index).RotCalc;
    }

    public void SetDefaults()
    {
        Thigh01.Enabled = true;
        Thigh01.Amp = 1f;
        Thigh02.Enabled = true;
        Thigh02.Amp = 0.30f;
        Thigh03.Enabled = true;
        Thigh03.Amp = 0.18f;
        Leg02.Enabled = true;
        Leg02.Amp = 0.03f;
        for (int i = 0; i < 4; i++)
        {
            PerBoneAmount amount = Get(i);
            amount.AxisX = 1f;
            amount.AxisY = 1f;
            amount.AxisZ = 1f;
            amount.RotCalc = true;
            amount.RotAmp = 0.25f;
        }
    }

    public void SetChainDefaults()
    {
        Thigh01.Enabled = true;
        Thigh01.Amp = 1f;
        Thigh02.Enabled = true;
        Thigh02.Amp = 0.8f;
        Thigh03.Enabled = true;
        Thigh03.Amp = 0.5f;
        Leg02.Enabled = true;
        Leg02.Amp = 0.12f;
        for (int i = 0; i < 4; i++)
        {
            PerBoneAmount amount = Get(i);
            amount.AxisX = 1f;
            amount.AxisY = 1f;
            amount.AxisZ = 1f;
            amount.RotCalc = true;
        }
    }
}
