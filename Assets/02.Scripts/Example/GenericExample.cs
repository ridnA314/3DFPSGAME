using UnityEngine;

//Generic is regardless of Type
//-> when use DS, do not define Data Type that DS used

public class Water
{
}

public class Coke
{
}

public class Cup<T>
{
    public T Drink;

    public Cup(T drink)
    {
        Drink = drink;
    }
}

public class GenericExample : MonoBehaviour
{
    private void Start()
    {
        Cup<int> cup1 = new Cup<int>(17);
        Cup<float> cup2 = new Cup<float>(0.2f);
        Cup<string> cup3 = new Cup<string>("Pepsi zero lime flavour");
        Cup<bool> cup4 = new Cup<bool>(true);
        Cup<Water> cup5 = new Cup<Water>(new Water());
        Cup<Coke> cup6 = new Cup<Coke>(new Coke());
    }
}