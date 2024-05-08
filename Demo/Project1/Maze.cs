using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    /// <summary>
    /// Represents a vertex in a 2D maze
    /// </summary>
    class Vertex
    {
        // Properties
        public int X { get; private set; }
        public int Y { get; private set; }
        public MazeTile Data { get; private set; }
        public bool Visited { get; set; }
        public List<Vertex> Adjcent {  get; private set; } = new List<Vertex>();
        public Vertex previous { get;  set; }

        // Constructor
        public Vertex(int x, int y, MazeTile data)
        {
            X = x;
            Y = y;
            Data = data;
            Visited = false;
        }
    }

    /// <summary>
    /// Possible types of tiles in the maze
    /// </summary>
    enum MazeTile
    {
        Empty,
        Wall,
        Start,
        End
    }

    /// <summary>
    /// A graph representation of a maze, read directly from an image file
    /// </summary>
    class Maze
    {
        // ==============================================
        //  DRAWING Fields - You won't need to use these
        // ==============================================

        // Size of each maze tile
        const int MazeUnitSize = 10;

        // Maze tile colors
        private Color MazeColorEmpty = Color.White;
        private Color MazeColorWall = Color.Black;
        private Color MazeColorStart = Color.Lime;
        private Color MazeColorEnd = Color.Red;
        private Color[] MazeColors;

        // Color of the path drawn on top of the maze
        private Color MazeColorPath = Color.CornflowerBlue;

        // A 1x1 white texture (basically a single pixel) for drawing
        private Texture2D pixel;

        // The maze offsets (using for centering the maze)
        int centeringOffsetX;
        int centeringOffsetY;



        // ==============================================
        //  MAZE Fields - These will be useful to you!
        // ==============================================

        // The maze sizes
        // (number of tiles in the maze)
        private int mazeSizeX;          // width
        private int mazeSizeY;          // height

        // The maze Vertices
        // (this class's Graph and "Adjacency" structure, all rolled into one handy-dandy thing!)
        private Vertex[,] vertices;

        // Starting and ending vertices for maze solution
        private Vertex startVertex;     // drawn in green
        private Vertex endVertex;       // drawn in red



        // ==============================================
        //  CONSTRUCTOR -  No need to edit this
        // ==============================================

        /// <summary>
        /// Creates a new maze object
        /// </summary>
        /// <param name="device">The graphics device for the game</param>
        public Maze(GraphicsDevice device, Texture2D mazeTexture, int screenWidth, int screenHeight)
        {
            // Create the 1x1 white pixel texture
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            // Set up the color array
            MazeColors = new Color[4];
            MazeColors[(int)MazeTile.Empty] = MazeColorEmpty;
            MazeColors[(int)MazeTile.Wall] = MazeColorWall;
            MazeColors[(int)MazeTile.Start] = MazeColorStart;
            MazeColors[(int)MazeTile.End] = MazeColorEnd;

            // Get the maze sizes
            mazeSizeX = mazeTexture.Width;
            mazeSizeY = mazeTexture.Height;

            // Offsets (for centering)
            centeringOffsetX = (screenWidth - MazeUnitSize * mazeSizeX) / 2;
            centeringOffsetY = (screenHeight - MazeUnitSize * mazeSizeY) / 2;

            // Get the data from the maze texture
            Color[] textureData = new Color[mazeTexture.Width * mazeTexture.Height];
            mazeTexture.GetData<Color>(textureData);

            // Set up the Vertices
            vertices = new Vertex[mazeSizeX, mazeSizeY];
            for (int y = 0; y < mazeSizeY; y++)
            {
                for (int x = 0; x < mazeSizeX; x++)
                {
                    // Get the color of this pixel
                    Color currentColor = textureData[y * mazeSizeX + x];

                    // Convert the color to maze data
                    MazeTile data = MazeTile.Empty;
                    if (currentColor == MazeColorWall) data = MazeTile.Wall;
                    else if (currentColor == MazeColorStart) data = MazeTile.Start;
                    else if (currentColor == MazeColorEnd) data = MazeTile.End;

                    // Set up this Vertex and check for start/end
                    vertices[x, y] = new Vertex(x, y, data);
                    if (data == MazeTile.Start) startVertex = vertices[x, y];
                    if (data == MazeTile.End) endVertex = vertices[x, y];
                }
            }
        }



        // ==============================================
        //  GIVEN METHODS - No need to edit these
        // ==============================================

        /// <summary>
        /// Draws the maze on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Ensure the data is reset
            ResetAllVertices();

            // Draw the maze, then solve and draw the solution
            DrawMaze(spriteBatch);
            DrawSolution(spriteBatch, SolveMaze());
        }

        /// <summary>
        /// Draws the walls, start and end locations of the map
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMaze(SpriteBatch spriteBatch)
        {
            // The rectangle to use for drawing
            Rectangle rect = new Rectangle();
            rect.Width = MazeUnitSize;
            rect.Height = MazeUnitSize;

            // Draw each tile, properly centered on the screen
            for (int x = 0; x < mazeSizeX; x++)
            {
                for (int y = 0; y < mazeSizeY; y++)
                {
                    // Set up the rectangle
                    rect.X = x * MazeUnitSize + centeringOffsetX;
                    rect.Y = y * MazeUnitSize + centeringOffsetY;

                    // Draw
                    spriteBatch.Draw(pixel, rect, MazeColors[(int)vertices[x, y].Data]);
                }
            }
        }

        /// <summary>
        /// Draw the solution path
        /// </summary>
        /// <param name="spriteBatch">Used for drawing</param>
        /// <param name="solution">The list that represents the solution</param>
        public void DrawSolution(SpriteBatch spriteBatch, List<Vertex> solution)
        {
            // Set up the rectangle
            Rectangle rect = new Rectangle();
            rect.Width = MazeUnitSize;
            rect.Height = MazeUnitSize;

            // Loop until we're out of Vertices
            foreach (Vertex currentVertex in solution)
            {
                // Skip start and end tiles so we can still see them
                if (currentVertex.Data == MazeTile.Start ||
                    currentVertex.Data == MazeTile.End)
                    continue;

                // Set up this rectangle
                rect.X = currentVertex.X * MazeUnitSize + centeringOffsetX;
                rect.Y = currentVertex.Y * MazeUnitSize + centeringOffsetY;

                // Draw
                spriteBatch.Draw(pixel, rect, MazeColorPath);
            }
        }

        /// <summary>
        /// Sets all Vertices to "not visited"
        /// </summary>
        public void ResetAllVertices()
        {
            for (int x = 0; x < mazeSizeX; x++)
                for (int y = 0; y < mazeSizeY; y++)
                    vertices[x, y].Visited = false;
        }



        // ====================================================
        //  STUDENT METHODS - Your job is to complete these
        // ====================================================

        // Complete the IsTileValid method below:
        //    - It determines if the tile at [x,y] is a "valid" tile to search.
        //    - Valid means the indices are within the graph's boundaries, the tile is 
        //      open (not a wall) and it is not yet visited.
        //	  - It does NOT check adjacency - you'll need to do that yourself
        //	    in either SolveMaze() or another helper method.

        /// <summary>
        /// Checks a vertex to see if it is explorable
        /// </summary>
        /// <param name="x">The x value of the vertex to check</param>
        /// <param name="y">The y value of the vertex to check</param>
        /// <returns>True if the vertex is valid, false otherwise</returns>
        public Boolean IsTileValid(int x, int y)
        {
            // -------------------------------------------------------------------------
            // Determine if a Tile is a valid tile to visit!
            // -------------------------------------------------------------------------

            if (x >= 0 && x < mazeSizeX && y >= 0 && y < mazeSizeY && vertices[x, y].Data != MazeTile.Wall && vertices[x, y].Visited == false)
            {
                return true;
            }
            return false;
        }


        // Complete the SolveMaze method below:
        // It must return a list of vertices on the path from start to the exit.
        // This is NOT Dijkstra's algorithm - Just a normal graph search.
        // 
        // Some useful information/tips:
        // 
        // * Feel free to write any helper methods you think would be helpful!
        // 
        // * All vertices are stored in a 2D array: vertices[x, y]
        //    - This is the Graph for the maze.
        //    - This is NOT an adjacency matrix.
        //    - In fact, there is no adjacency matrix or adjacency list.
        //    - Neither are needed for a grid-based unweighted graph.
        //    - But determining adjacency on a grid is fairly uncomplicated!
        //    
        // * Assume only cardinal directions are adjacent. Diagonals are not adjacent.  
        // 
        // * The Maze already knows about the start and end vertices, 
        //   which are stored in the following fields:
        //    - startVertex
        //    - endVertex
        //    
        // * The end condition is finding the "endVertex" during the search.
        //   Once it is reached, end the search and use the vertices
        //    currently in your data structure as the path.

        /// <summary>
        /// Runs an appropriate graph search on this maze.
        /// </summary>
        /// <returns>List of Vertices that represents the solution from start to end.</returns>
        public List<Vertex> SolveMaze()
        {
            // -------------------------------------------------------------------------
            // 1. Use either Depth-First Search or Breadth-First Search to solve this maze.
            //    - One of them is more appropriate for this task.
            // LEAVE A COMMENT answering the two following questions:
            //    - Which one are you using?  
            //    - Which data structure is needed for that type of search?
            // -------------------------------------------------------------------------
            
            // YOUR ANSWER:
            // Breadth-First Search

            // -------------------------------------------------------------------------
            // 2. UNCOMMENT & use the SINGLE MOST APPROPRIATE structure below for the search.
            // DELETE the remaining two unused ones.
            // -------------------------------------------------------------------------
            
            Queue<Vertex> queue = new Queue<Vertex>();

            // -------------------------------------------------------------------------
            // 3. COMPLETE THE ITERATIVE GRAPH SEARCH HERE.
            // -------------------------------------------------------------------------

            SetAdjcent();
            queue.Enqueue(startVertex);
            startVertex.Visited = true;
            while (queue.Count > 0)
            {
                Vertex current = queue.Dequeue();
                if (current == endVertex)
                {
                    break;
                }
                Vertex next = GetUnvisitedAdj(current);
                while (next != null)
                {
                    next.Visited = true;
                    next.previous = current;
                    queue.Enqueue(next);
                    next = GetUnvisitedAdj(current);
                }
            }

            // -------------------------------------------------------------------------
            // 4. ADD PATH TO SOLUTION LIST.
            //    - Add vertices found during the search to the "path" List below
            //    - You can use the path list's AddRange() method as necessary
            // -------------------------------------------------------------------------
            
            List<Vertex> path = new List<Vertex>();
            Vertex temp = endVertex;
            while (temp != startVertex)
            {
                path.Add(temp);
                temp = temp.previous;
            }
            path.Add(startVertex);
            path.Reverse();

            // -------------------------------------------------------------------------
            // 5. You are all done!
            // This method now returns the full "path" to solve the maze
            // DO NOT change this return statement.
            
            return path;

            // -------------------------------------------------------------------------
        }

        // You may write additional helper methods, too.

        /// <summary>
        /// method to set adjcent tiles for each tile
        /// </summary>
        public void SetAdjcent()
        {
            for (int x = 0; x < mazeSizeX; x++)
            {
                for (int y = 0; y < mazeSizeY; y++)
                {
                    if (IsTileValid(x + 1, y))
                    {
                        vertices[x, y].Adjcent.Add(vertices[x + 1, y]);
                    }
                    if (IsTileValid(x - 1, y))
                    {
                        vertices[x, y].Adjcent.Add(vertices[x - 1, y]);
                    }
                    if (IsTileValid(x, y + 1))
                    {
                        vertices[x, y].Adjcent.Add(vertices[x, y + 1]);
                    }
                    if (IsTileValid(x, y - 1))
                    {
                        vertices[x, y].Adjcent.Add(vertices[x, y - 1]);
                    }
                }
            }
        }

        /// <summary>
        /// method to get unvisited adjcent tile
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public Vertex GetUnvisitedAdj(Vertex current)
        {
            foreach (Vertex adj in current.Adjcent)
            {
                if (adj.Visited == false)
                {
                    return adj;
                }
            }
            return null;
        }
    }

}

