using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Boardgame_Sample
{
    internal struct Coord
    {
        public int x, y;

        public Coord(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public static bool operator ==(Coord a, Coord b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Coord a, Coord b) => a.x != b.x || a.y != b.y;
    }

    internal class Node
    {
        public Node(int x, int y)
        {
            coord = new Coord(x, y);
        }
        public Coord coord;
        public Unit? unit;
    }

    internal class Unit
    {
        public int layer;
        public int hp;
        public int atk;
        public int speed;
        public int attackRange;
    }

    internal class Board
    {
        public Board(Coord maxCoord)
        {
            nodes = new Node[maxCoord.x + 1, maxCoord.y + 1];

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }
        }

        public Node[,] nodes;

        public void SpawnUnit(int x, int y, Unit unit)
        {
            nodes[x, y].unit = unit;
        }

        public async Task StartSimulationAsync(int tickDelayMS = 0)
        {
            bool isFinished = false;

            while (isFinished == false)
            {
                DisplayMap();
                isFinished = SimulationTick();
                await Task.Delay(tickDelayMS);
            }
        }

        public bool SimulationTick()
        {
            bool isFinished = false;

            var movementBuffer = new ConcurrentDictionary<Coord, Coord>();
            var attackBuffer = new ConcurrentBag<(Coord target, Coord source)>();

            // 병렬 처리를 사용하여 모든 유닛의 행동을 처리
            Parallel.For(0, nodes.GetLength(0), i =>
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Unit unit = nodes[i, j].unit;

                    if (unit != null)
                    {
                        Coord currentCoord = new Coord(i, j);

                        Node targetNode = FindNodeInRange(currentCoord, unit.attackRange, node => node.unit != null && node.unit.layer != unit.layer);

                        if (targetNode != null)
                        {
                            attackBuffer.Add((targetNode.coord, new Coord(i, j)));
                        }
                        else
                        {
                            Node nearestTargetNode = FindNearestTarget(currentCoord, unit.layer);

                            if (nearestTargetNode != null)
                            {
                                Coord targetCoord = nearestTargetNode.coord;
                                Coord newCoord = GetNextStepTowards(currentCoord, targetCoord);

                                if (newCoord != currentCoord && nodes[newCoord.x, newCoord.y].unit == null)
                                {
                                    movementBuffer.AddOrUpdate(newCoord, new Coord(i, j),
                                        (key, oldCoord) =>
                                        {
                                            Unit alreadyUnit = nodes[oldCoord.x, oldCoord.y].unit;
                                            Unit newUnit = nodes[i, j].unit;

                                            return (newUnit.speed > alreadyUnit.speed) ? new Coord(i, j) : oldCoord;
                                        });
                                }
                            }
                        }
                    }
                }
            });

            foreach (var buffer in movementBuffer)
            {
                MovementUnit(buffer.Key, buffer.Value);
            }

            foreach (var buffer in attackBuffer)
            {
                AttackUnit(buffer.target, buffer.source);
            }

            if (IsBattleOver())
            {
                isFinished = true;
            }

            return isFinished;
        }

        private void MovementUnit(Coord newCoord, Coord currentCoord)
        {
            Unit unit = nodes[currentCoord.x, currentCoord.y].unit;

            if (unit == null)
                return;

            nodes[newCoord.x, newCoord.y].unit = unit;
            nodes[currentCoord.x, currentCoord.y].unit = null;
        }

        private void AttackUnit(Coord target, Coord source)
        {
            Unit targetUnit = nodes[target.x, target.y].unit;
            Unit sourceUnit = nodes[source.x, source.y].unit;

            if (targetUnit == null || sourceUnit == null)
                return;

            nodes[target.x, target.y].unit.hp -= sourceUnit.atk;

            if (targetUnit.hp <= 0)
            {
                nodes[target.x, target.y].unit = null;
            }
        }

        public Node FindNodeInRange(Coord coord, int range, Func<Node, bool> condition)
        {
            for (int dx = -range; dx <= range; dx++)
            {
                int remainingRange = range - Math.Abs(dx);

                for (int dy = -remainingRange; dy <= remainingRange; dy++)
                {
                    int x = coord.x + dx;
                    int y = coord.y + dy;

                    if (x < 0 || x >= nodes.GetLength(0) || y < 0 || y >= nodes.GetLength(1))
                        continue;

                    Node node = nodes[x, y];

                    if (condition.Invoke(node))
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        public Node FindNearestTarget(Coord coord, int layer)
        {
            Node nearestNode = null;
            int nearestDistance = int.MaxValue;

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Node node = nodes[i, j];

                    if (node.unit != null && node.unit.layer != layer)
                    {
                        int distance = Math.Abs(coord.x - i) + Math.Abs(coord.y - j);

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestNode = node;
                        }
                    }
                }
            }

            return nearestNode;
        }

        public Coord GetNextStepTowards(Coord current, Coord target)
        {
            int dx = target.x - current.x;
            int dy = target.y - current.y;

            Coord moveX = new Coord(current.x + Math.Sign(dx), current.y);
            Coord moveY = new Coord(current.x, current.y + Math.Sign(dy));

            if (nodes[moveX.x, moveX.y].unit == null)
            {
                return moveX;
            }
            else if (nodes[moveY.x, moveY.y].unit == null)
            {
                return moveY;
            }

            return current;
        }

        public bool IsBattleOver()
        {
            bool[] layersPresent = new bool[2];

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    Node node = nodes[i, j];
                    if (node.unit != null)
                    {
                        layersPresent[node.unit.layer] = true;
                    }
                }
            }

            return layersPresent.Count(x => x) <= 1;
        }

        public void DisplayMap()
        {
            StringBuilder sb = new StringBuilder();

            Console.WriteLine("--------------Render---------------");
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (nodes[i, j].unit != null)
                    {
                        int maxHp = 10;
                        double healthRatio = (double)nodes[i, j].unit.hp / maxHp;

                        if (nodes[i, j].unit.layer == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("♡ ");
                        }
                        else if (nodes[i, j].unit.layer == 1)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.Write("♣ ");
                        }
                        sb.AppendLine($"{(healthRatio * 100):0.0}% HP at ({i}, {j})");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("□ ");
                    }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(sb);
        }
    }
}
