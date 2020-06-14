public class Block {

    public static readonly int textureSheetSize = 2; // 2x2 grid of textures
    public static readonly float cellSize = 1f / textureSheetSize;

    public string type;
    public bool isSolid;
    // Back, Front, Top, Bottom, Left, Right
    public int[] textures;

    public Block(string type, bool isSolid, int[] textures) {
        this.type = type;
        this.isSolid = isSolid;
        this.textures = textures;
    }

    public static Block[] GetBlocks() {
        return new Block[] {
            new Block("air", false, new int[] {}), // Block 0
            new Block("dirt", true, new int[] {1, 1, 2, 1, 1, 1}), // Block 1
            new Block("stone", true, new int[] {0, 0, 0, 0, 0, 0}), // Block 2
            new Block("sand", true, new int[] {3, 3, 3, 3, 3, 3}), // Block 3
        };
    }

}
