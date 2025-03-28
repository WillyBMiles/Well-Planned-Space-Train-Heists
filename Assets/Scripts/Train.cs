using System.Collections.Generic;
using System.Text;

public class Train 
{
    public List<TrainPart> trainParts = new();

    public Train(System.Random random)
    {

        trainParts.Add(new FrontTrain());
        int numberOfAdditionalParts = random.Next(4, 8);
        for (int i =0; i < numberOfAdditionalParts; i++)
        {
            TrainPart part;
            if (random.Next(10) < 4f)
            {
                part = new DefenderTrain();
            }
            else
            {
                var cargoTrain = new CargoTrain
                {
                    numberOfRewardCubes = random.Next(1, 6)
                };
                part = cargoTrain;
            }
            trainParts.Add(part);
        }

    }

    public void Generate()
    {

    }

    public string Stringify()
    {
        StringBuilder builder = new();
        foreach (var part in trainParts)
        {
            builder.AppendLine(part.Serialize());

        }
        return builder.ToString();
    }
}
