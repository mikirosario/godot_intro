public interface IDamageable
{
    uint Health { get; }
    uint ReceiveDamage (IDamager damager);
}

public interface IDamager
{
    uint Attack { get; }
}
