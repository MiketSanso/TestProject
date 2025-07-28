public interface ICubeFactory
{
    ICubeView CreateCube(bool isInTower);
    ICubeView GetCubeView(CubeModel model);
}