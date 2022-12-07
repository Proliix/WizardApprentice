using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    public void GetStunned(float stunDuration = 0.25f);

    IEnumerator IsStunned(float stunDuration = 0.25f);

}
