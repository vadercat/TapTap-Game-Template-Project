namespace TapTap
{
    public interface IPoolable
    {
        void OnPoolPushEvent();

        void OnPoolPopEvent();
    }
}
