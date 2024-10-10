
namespace Boardgame_Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Board board = new Board(new Coord(4, 4));

            board.SpawnUnit(1, 1, new Unit()
            {
                layer = 0,
                hp = 10,
                atk = 1,
                attackRange = 1,
                speed = 1,
            });
            board.SpawnUnit(1, 0, new Unit()
            {
                layer = 0,
                hp = 5,
                atk = 1,
                attackRange = 3,
                speed = 1,
            });
            board.SpawnUnit(3, 3, new Unit()
            {
                layer = 1,
                hp = 10,
                atk = 1,
                attackRange = 1,
                speed = 1,
            });
            board.SpawnUnit(3, 4, new Unit()
            {
                layer = 1,
                hp = 5,
                atk = 1,
                attackRange = 3,
                speed = 1,
            });

            await board.StartSimulationAsync(1000);

            //bool isFinished = false;
            //while (isFinished == false)
            //{
            //    board.DisplayMap();
            //    isFinished = board.SimulationTick();
            //    //await Task.Delay(tickDelayMS);
            //}
            //board.DisplayMap();
        }
    }
}
