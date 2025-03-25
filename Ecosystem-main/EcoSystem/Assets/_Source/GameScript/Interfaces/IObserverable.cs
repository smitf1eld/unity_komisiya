namespace _Source.GameScript.Interfaces
{
    public interface IObserverable
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers();
    }
}