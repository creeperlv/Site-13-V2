namespace Site13Kernel.Core.TagSystem
{
    public class SystemBase:ControlledBehavior
    {
        public EntityCollection Collection=new EntityCollection();
        public override void Init()
        {

        }
        public virtual void Execute(float DT, float UDT)
        {
        }
    }
}
