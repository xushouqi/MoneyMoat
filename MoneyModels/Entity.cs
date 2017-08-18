

namespace MoneyModels
{
    public abstract class Entity 
    {
        public abstract int GetId();
        public abstract System.DateTime TryUpdateTime();
    }
}
