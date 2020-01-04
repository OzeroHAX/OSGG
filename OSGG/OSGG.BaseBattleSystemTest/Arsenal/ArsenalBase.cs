using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OSGG.BaseBattleSystemTest.Arsenal
{
    /// <summary>
    /// Базовый класс боевой единицы
    /// </summary>
    public abstract class ArsenalBase
    {
        #region Specifications
        /// <summary>
        /// Скорострельность
        /// </summary>
        public abstract int RateOfFireInMinute { get; }
        /// <summary>
        /// Мощность щита
        /// </summary>
        public abstract int SheildPower { get; }
        /// <summary>
        /// Мощность выстрела
        /// </summary>
        public abstract int ShotPower { get; }
        /// <summary>
        /// Сила брони
        /// </summary>
        public abstract int ArmorPower { get; }
        #endregion

        #region CurrentState
        /// <summary>
        /// Текущее состояние щита
        /// </summary>
        public float CurrentSheild { get; set; }
        /// <summary>
        /// Текущее состояние оружия
        /// </summary>
        public float CurrentShoutPower { get; set; }
        /// <summary>
        /// Текущее состояние брони
        /// </summary>
        public float CurrentArmor { get; set; }
        /// <summary>
        /// Уничтожен ли корабль?
        /// </summary>
        public bool IsDestroy { get => CurrentArmor <= 0; }
        #endregion

        public ArsenalBase()
        {
            CurrentShoutPower = ShotPower;
            CurrentArmor = ArmorPower;
            CurrentSheild = SheildPower;
        }

        public void GetDamage(float damagePower)
        {
            
            if (IsDestroy) return;

            if (IsDamageLow(damagePower))
            {
                UpdateCurrentSheildLowDamage(damagePower);
                return;
            }

            float residueDamage = CalculateResidueDamage(damagePower);

            UpdateCurrentSheildHightDamage(residueDamage);

            UpdateCurrentArmor(residueDamage);

            UpdateCurrentShoutPower();
        }

        private bool IsDamageLow(float damagePower)
            => CurrentSheild >= damagePower;

        private void UpdateCurrentSheildLowDamage(float damagePower)
        {
            UpdateCurrentSheildFromFunc(() => damagePower / 10);
        }

        private void UpdateCurrentSheildHightDamage(float residueDamage)
        {
            UpdateCurrentSheildFromFunc(() => residueDamage / 2);
        }

        private void UpdateCurrentSheildFromFunc(Func<float> currentSheildCalculateFunc)
        {
            if (CurrentSheild == 0) return;
            CurrentSheild -= currentSheildCalculateFunc();
            if (CurrentSheild < 0) CurrentSheild = 0;
        }

        private float CalculateResidueDamage(float damagePower)
            => damagePower - CurrentSheild;

        private void UpdateCurrentArmor(float residueDamage)
        {
            CurrentArmor -= residueDamage;
        }

        private void UpdateCurrentShoutPower()
        {
            if (IsCurrentArmorLow())
            {
                CurrentShoutPower = CurrentShoutPower / 100 * 80;
            }
        }
        private bool IsCurrentArmorLow()
            => CurrentArmor / ArmorPower * 100 < 20;

    }
}
