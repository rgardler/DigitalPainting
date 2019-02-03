using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace wizardscode.statistics
{
    [Serializable]
    public class Statistic
    {
        public float BaseValue;

        public float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;
        protected float _value;
        protected float lastBaseValue = float.MinValue;

        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        public Statistic()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public Statistic(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifiersOrder);
        }

        protected int CompareModifiersOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
            {
                return -1;
            }
            else if (a.Order > b.Order)
            {
                return 1;
            }
            return 0;
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i == 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    statModifiers.RemoveAt(i);
                    didRemove = true;
                }
            }

            return didRemove;
        }

        protected float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                switch (mod.Type)
                {
                    case StatModifierType.Flat:
                        finalValue += statModifiers[i].Value;
                        break;
                    case StatModifierType.PercentAdditive:
                        sumPercentAdd += mod.Value;

                        if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModifierType.PercentAdditive)
                        {
                            finalValue *= sumPercentAdd;
                        }
                        break;
                    case StatModifierType.PercentMultiplicative:
                        finalValue *= 1 + mod.Value;
                        break;
                    default:
                        Debug.LogWarning("Don't know how to apply StatModifier of type " + mod.Type);
                        break;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}