using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClause : MonoBehaviour
{
    [SerializeField] float clauseTime = 3;
    MenuObjectScript menu;

    private void Start()
    {
        menu = GetComponent<MenuObjectScript>();
    }

    // Update is called once per frame
    void Update()
    {
        clauseTime -= Time.unscaledDeltaTime;
        if (clauseTime <= 0)
        {
            menu.Clause();
        }
    }
}
