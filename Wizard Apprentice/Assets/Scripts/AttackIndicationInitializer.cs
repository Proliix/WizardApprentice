using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicationInitializer : MonoBehaviour
{
    [SerializeField] Sprite squareSprite;
    [SerializeField] Sprite circleSprite;
    // Start is called before the first frame update
    void Start()
    {
        AttackIndicator.squareSprite = squareSprite;
        AttackIndicator.circleSprite = circleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
