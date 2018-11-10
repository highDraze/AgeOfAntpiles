using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position {

    public int x; // Gehört an zweiter Stelle im grossen Datenarray
    public int y;

    public bool ignore;

    public Position predecessor;
    public double f;
    public double g;

    public Position() {
        //predecessor = new Position(- 1, -1);
        g = 0.0;
        this.ignore = false;
    }
    public Position(int x, int y) {
        this.x = x;
        this.y = y;
        this.ignore = false;
        g = 0.0;
        //predecessor = new Position(-1, -1);
    }
    public Position(int x, int y, int predX, int predY) {
        this.x = x;
        this.y = y;
        this.ignore = false;

        predecessor = new Position(predX, predY);
        g = 0.0;
    }

    public bool equals(Position other) {
        if (this.x == other.x && this.y == other.y) {
            return true;
        }
        return false;
    }
}
