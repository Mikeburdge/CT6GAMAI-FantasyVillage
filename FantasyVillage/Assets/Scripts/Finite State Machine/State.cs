using Assets.Scripts;
using System.Collections;

public abstract class State
{
    public virtual IEnumerator GoHome()
    {
        yield break;
    }
    public virtual IEnumerator BeIdle()
    {
        yield break;
    }
    public virtual IEnumerator WorkDefaultProfession()
    {
        yield break;
    }
    public virtual IEnumerator WorkSpecificProfession(Professions professions)
    {
        yield break;
    }
}
    