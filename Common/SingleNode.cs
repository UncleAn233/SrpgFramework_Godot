using Godot;

namespace SrpgFramework.Common
{
    public partial class SingleNode<T> : Node where T : class
    {
        private static T _instance;
        public static T Instance => _instance;

        public override void _Ready()
        {
            if (_instance is not null)
            {
                this.QueueFree();
                return;
            }
            _instance = this as T;
        }
    }
}