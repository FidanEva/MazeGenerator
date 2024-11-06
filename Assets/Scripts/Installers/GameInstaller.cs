using PathFinding;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ProceduralMazeGeneration.ProceduralMazeGeneration>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PathFinding.PathFinding>().AsSingle()
            .WithArguments(Container.Resolve<ProceduralMazeGeneration.ProceduralMazeGeneration>());
        Container.Bind<Agent>().FromComponentInHierarchy().AsTransient();
    }
}