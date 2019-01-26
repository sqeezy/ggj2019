public class Tile : Revealable
{
	public bool Walkable;
	public int X;

	public int Y;

	public override string ToString()
	{
		return $"({X} | {Y})";
	}
}