using System;

[Serializable]
public class Champion
{
    public string Name;
    public bool IsDone;

    public Champion(string name, bool isDone = false)
    {
        Name = name;
        IsDone = isDone;
    }
}