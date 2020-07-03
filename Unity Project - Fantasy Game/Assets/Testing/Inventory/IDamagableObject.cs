using System;
using System.Collections.Generic;
using UnityEngine;
public interface IDamagableObject
{
    void DamageObj(int dmg, EntityStatus sender);
}
