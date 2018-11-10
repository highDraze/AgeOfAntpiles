using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinder {
    Position destination;
    Position start;
    List<Position> openList;
    List<Position> closedList;
    bool workingAtEarth;
    BlockInfo[,] blockInfo;


    public Pathfinder(GameData gameData) {
        blockInfo = gameData.blockInfos;
    }


    public Queue<Position> pathfindingAlgo(Position start, Position destination, bool workerAnt = false) {
        //Debug.Log(blockInfo[1,7].material);
        // Init
        workingAtEarth = workerAnt;
        if(workingAtEarth) {
            if(!checkIfEarth(destination.x, destination.y)) {
                workingAtEarth = false;
            }
        }
        this.destination = start;
        this.start = destination;
        this.start.g = 0;
        this.start.predecessor = new Position(-1, -1); //
        //Debug.Log(": X: " + this.start.x + ", Y: " + this.start.predecessor.y + ", pred X: " + this.start.predecessor.x + ", pred Y: " + this.start.predecessor.y);
        openList = new List<Position>(0);
        closedList = new List<Position>(0);
        openList.Add(this.start);

        // Algo
        bool pathFound = false;
        // Check if destination is earth
        /*
        if(!workerAnt || !checkIfEarth(destination.x, destination.y)) {
            return new Queue<Position>(0);
        }
        */
        while (openList.Count != 0) {
            
            // Knoten mit dem geringsten f-Wert aus der Open List entfernen
            Position currentPos = findMin();
            //Debug.Log(": X: " + currentPos.x + ", Y: " + currentPos.y);// + ", pred X: " + currentPos.predecessor.x + ", pred Y: " + currentPos.predecessor.y);
            // Wurde das Ziel gefunden?
            if (currentPos.equals(this.destination)) {
               // Debug.Log(currentPos.x + " " + currentPos.y);
                // || (workerAnt && checkIfEarth(currentPos.x, currentPos.y))) { 
                this.destination.predecessor = currentPos.predecessor;
                pathFound = true;

                break;
            }
            // Der aktuelle Knoten soll durch nachfolgende Funktionen
            // nicht weiter untersucht werden, damit keine Zyklen entstehen
            closedList.Add(currentPos);
            // Wenn das Ziel noch nicht gefunden wurde: Nachfolgeknoten
            // des aktuellen Knotens auf die Open List setzen
            expandNode(currentPos);
        }


        if(pathFound == false) {
            Debug.Log("Path not found!");
            return new Queue<Position>(0);
        }
        else {
            //Debug.Log("Am Ende mit " + closedList.Count + " Objekten in closedList:");
            for(int i = 0; i < closedList.Count; i++) {
                //Debug.Log(i + ": X: " + closedList[i].x + ", Y: " + closedList[i].y + ", pred X: " + closedList[i].predecessor.x + ", pred Y: " + closedList[i].predecessor.y);
            }
            return createQueue();
        }
    }

    void expandNode(Position currentPos) {
        List<Position> successors = new List<Position>();
        // Adding Elements to successors
        if (checkIfFree(currentPos.x + 1, currentPos.y)) {
            Position x = new Position(currentPos.x + 1, currentPos.y);
            //if(!currentPos.predecessor.equals(x)) {
                successors.Add(x);
            //}
        }
        if (checkIfFree(currentPos.x, currentPos.y + 1)) {
            Position x = new Position(currentPos.x, currentPos.y + 1);
            //if (!currentPos.predecessor.equals(x)) {
                successors.Add(x);
            //}
        }
        if (currentPos.x > 0 && checkIfFree(currentPos.x - 1, currentPos.y)) {
            Position x = new Position(currentPos.x - 1, currentPos.y);
            //if (!currentPos.predecessor.equals(x)) {
                successors.Add(x);
            //}
        }
        if (currentPos.y > 0 && checkIfFree(currentPos.x, currentPos.y - 1)) {
            Position x = new Position(currentPos.x, currentPos.y - 1);
            //if (!currentPos.predecessor.equals(x)) {
                successors.Add(x);
            //}
        }



        for (int i = 0; i < successors.Count; i++) {
            // wenn der Nachfolgeknoten bereits auf der Closed List ist – tue nichts
            if (alreadyInClosedList(successors[i])) { continue; }
            // g-Wert für den neuen Weg berechnen: g-Wert des Vorgängers plus
            // die Kosten der gerade benutzten Kante
            double tentativeG = currentPos.g + 1.0;
            // In openList?
            bool isInOpenList = alreadyInOpenList(successors[i]);            
            // wenn der Nachfolgeknoten bereits auf der Open List ist,
            // aber der neue Weg nicht besser ist als der alte – tue nichts
            if(isInOpenList && tentativeG >= successors[i].g) { continue; }
            // Vorgängerzeiger setzen und g Wert merken
            //successor.predecessor := currentNode
            successors[i].predecessor = currentPos;
            successors[i].g = tentativeG;
            // f-Wert des Knotens in der Open List aktualisieren
            // bzw. Knoten mit f-Wert in die Open List einfügen
            successors[i].f = tentativeG + calcH(successors[i]);
            if (isInOpenList) {
                correctOpenList(successors[i]);
                //Debug.Log("Hmm");
            }
            else {
                openList.Add(successors[i]);
                //Debug.Log("Added X: " + successors[i].x + ", Y: " + successors[i].y);
            }

        }
        /*
        foreach successor of currentNode
            // wenn der Nachfolgeknoten bereits auf der Closed List ist – tue nichts
            if closedlist.contains(successor) then
                continue
            // g-Wert für den neuen Weg berechnen: g-Wert des Vorgängers plus
            // die Kosten der gerade benutzten Kante
            tentative_g = g(currentNode) + c(currentNode, successor)
            // wenn der Nachfolgeknoten bereits auf der Open List ist,
            // aber der neue Weg nicht besser ist als der alte – tue nichts
            if openlist.contains(successor) and tentative_g >= g(successor) then
                continue
            // Vorgängerzeiger setzen und g Wert merken
            successor.predecessor := currentNode
            g(successor) = tentative_g
            // f-Wert des Knotens in der Open List aktualisieren
            // bzw. Knoten mit f-Wert in die Open List einfügen
            f:= tentative_g + h(successor)
            if openlist.contains(successor) then
                openlist.decreaseKey(successor, f)
            else
                openlist.enqueue(successor, f)
        */
    }

    Queue<Position> createQueue() {
        Position currentPos = destination;
        List<Position> queue = new List<Position>();
        queue.Add(currentPos);
        while(currentPos.predecessor.x != -1 && currentPos.predecessor.y != -1) {
            //Debug.Log("In createQueue: Pred X: " + currentPos.predecessor.x + ", Y: " + currentPos.predecessor.y);
            Position newPos = findPredecessor(currentPos);
            queue.Add(newPos);
            currentPos = newPos;
        }
        analyzePath(queue);
        Queue<Position> queueFinal = new Queue<Position>(queue);
        return queueFinal;
    }

    void analyzePath(List<Position> queue) {
        int cutBeforeEnd = workingAtEarth ? 3 : 2;
        for (int i = 0; i < queue.Count - cutBeforeEnd; i++) {
            if (queue[i].ignore) continue;

            Position currentPos = queue[i];
            Position toCheckNext = queue[i + 2];
            // Is a cut possible
            if (!(currentPos.x + 1 == toCheckNext.x && currentPos.y + 1 == toCheckNext.y ||
                  currentPos.x + 1 == toCheckNext.x && currentPos.y - 1 == toCheckNext.y ||
                  currentPos.x - 1 == toCheckNext.x && currentPos.y + 1 == toCheckNext.y ||
                  currentPos.x - 1 == toCheckNext.x && currentPos.y - 1== toCheckNext.y
                )) continue;
            // Check blocks nearby, if both are free, blacklist next block
            if(currentPos.x + 1 == toCheckNext.x && currentPos.y + 1 == toCheckNext.y) {
                if(checkIfFree(currentPos.x + 1, currentPos.y) && checkIfFree(currentPos.x, currentPos.y + 1)) { queue[i + 1].ignore = true; continue; }
            }
            if (currentPos.x + 1 == toCheckNext.x && currentPos.y - 1 == toCheckNext.y) {
                if (checkIfFree(currentPos.x + 1, currentPos.y) && checkIfFree(currentPos.x, currentPos.y - 1)) { queue[i + 1].ignore = true; continue; }
            }
            if (currentPos.x - 1 == toCheckNext.x && currentPos.y + 1 == toCheckNext.y) {
                if (checkIfFree(currentPos.x - 1, currentPos.y) && checkIfFree(currentPos.x, currentPos.y + 1)) { queue[i + 1].ignore = true; continue; }
            }
            if (currentPos.x - 1 == toCheckNext.x && currentPos.y - 1 == toCheckNext.y) {
                if (checkIfFree(currentPos.x - 1, currentPos.y) && checkIfFree(currentPos.x, currentPos.y - 1)) { queue[i + 1].ignore = true; continue; }
            }

        }
    }





    // Helper methods
    Position findMin() {
        double minF = 100000.0;
        int minIndex = -1;
        for(int i = 0; i < openList.Count; i++) {
            if(openList[i].f < minF) {
                minF = openList[i].f;
                minIndex = i;
            }
        }
        Position minPos = openList[minIndex];
        openList.RemoveAt(minIndex);

        return minPos;
    }

    Position findPredecessor(Position currentPos) {
        int index = -1;
        for (int i = 0; i < closedList.Count; i++) {
            if (closedList[i].equals(currentPos.predecessor)) {
                index = i; break;
            }
        }
        Position predecessor = closedList[index];
        //openList.RemoveAt(index);

        return predecessor;
    }

    bool checkIfFree(int x, int y) {
       // Debug.Log("x: " + x + " y: " + y);
        if (blockInfo[y, x].material == 'n') {
            return true;
        }
        return false;
    }
    bool checkIfEarth(int x, int y) {
        if (blockInfo[y, x].material == 'e') {
            return true;
        }
        return false;
    }

    bool alreadyInClosedList(Position successor) {
        for (int i = 0; i < closedList.Count; i++) {
            if (successor.equals(closedList[i])) return true;
        }
        return false;
    }

    bool alreadyInOpenList(Position successor) {
        for (int i = 0; i < openList.Count; i++) {
            if (successor.equals(openList[i])) return true;
        }
        return false;
    }
    //Position makePosFromBlockInfo(int x, int y) {
    //    return new Position()
    //}

    double calcH(Position position) {
        return System.Math.Sqrt(System.Math.Pow(position.x, 2) + System.Math.Pow(position.y, 2));
    }

    void correctOpenList(Position successor) {
        for (int i = 0; i < openList.Count; i++) {
            if (successor.equals(openList[i])) {
                openList[i].g = successor.g;
                openList[i].f = successor.f;
                openList[i].predecessor = successor.predecessor;
            }
        }
    }

    // Just for tests
}
