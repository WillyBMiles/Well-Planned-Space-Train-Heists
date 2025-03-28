using UnityEngine;

public abstract class TrainPart
{
    public abstract float Distance { get; }
    public abstract TrainPartInstance Spawn();
    public abstract string Serialize();
}

public class FrontTrain : TrainPart
{
    public override float Distance => 12.61f;
    public override TrainPartInstance Spawn()
    {
        return ObjectPool.SpawnObject<TrainPartInstance>("FTrain");
    }

    public override string Serialize()
    {
        return "Lead Car";
    }
}

public class CargoTrain : TrainPart
{
    public override float Distance => 11.53f;
    public int numberOfRewardCubes;
    public override TrainPartInstance Spawn()
    {
        var e = ObjectPool.SpawnObject<TrainPartInstance>("CTrain");
        e.Enemy.numberOfCubesToDrop = numberOfRewardCubes;

        return e;
    }

    public override string Serialize()
    {
        return "Car Carrying: " + numberOfRewardCubes + " Cargo";
    }
}

public class DefenderTrain : TrainPart
{
    public override float Distance => 11.73f;
    public override TrainPartInstance Spawn()
    {
        return ObjectPool.SpawnObject<TrainPartInstance>("DTrain");
    }

    public override string Serialize()
    {
        return "Defender Car";
    }
}