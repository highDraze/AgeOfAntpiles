using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {

    public Position target;
    public char type;
    public bool build;

    public Order(Position target, bool build = false, char type = 'e')
    {
        this.build = build;
        this.target = target;
        this.type = type;
    }
}
