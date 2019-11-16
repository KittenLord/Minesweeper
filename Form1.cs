using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatrixLib;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        public bool IsExploring = true;

        public bool HasWonOrFailed = false;

        public int totalMineCount = 0;
        public int flagCount = 0;

        public Form1()
        {
            InitializeComponent();

            Matrix m = new Matrix(14,14); // Setting up matrix
            m.Setup(Controls, BlockColor.DefaultColors.Default);
            m.SetLinkedDataForAll(false, 0);
            MatrixColors.GetColorPack(ColorPacksTypes.MainColorPack);

            Random r = new Random(); // Setting up mines

            foreach (var b in m.Blocks)
            {
                if(r.Next(0,8) == 0)
                {
                    b.AddTags(false, "Mine");
                    totalMineCount++;
                }
            }



            for(int i = 0; i < m.Blocks.Count; i++)
            {
                if (m.Blocks[i].tags.Contains("Mine")) 
                {
                    continue;
                }

                var minesCount = m.GetNeighbours(m.Blocks[i]).Where(p => p.tags.Contains("Mine")).ToList();

                switch (minesCount.Count)
                {
                    case 0:
                        m.Blocks[i].AddTags(false, "None");
                        break;
                    case 1:
                        m.Blocks[i].AddTags(false, "One");
                        break;
                    case 2:
                        m.Blocks[i].AddTags(false, "Two");
                        break;
                    case 3:
                        m.Blocks[i].AddTags(false, "Three");
                        break;
                    case 4:
                        m.Blocks[i].AddTags(false, "Four");
                        break;
                    case 5:
                        m.Blocks[i].AddTags(false, "Five");
                        break;
                    case 6:
                        m.Blocks[i].AddTags(false, "Six");
                        break;
                    case 7:
                        m.Blocks[i].AddTags(false, "Seven");
                        break;
                    case 8:
                        m.Blocks[i].AddTags(false, "Eight");
                        break;
                    default:
                        break;
                }
            }

            List<Control> matrixBlocks = new List<Control>(); // Events

            foreach (Control c in Controls)
            {
                if (c.Name.Contains("matrixBlock")) matrixBlocks.Add(c);
            }

            foreach (var c in matrixBlocks)
            {
                c.Click += (s, e) =>
                {
                    if (HasWonOrFailed) return;

                    var block = m.Blocks.Find(p => p.block.Name == c.Name);

                    if (!IsExploring)
                    {
                        if ((bool)block.linkedData[0] == true) return;

                        if(block.tags.Contains("Flag"))
                        {
                            block.RemoveTags("Flag");
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.ClosedTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            flagCount--;
                            UpdateCounters();

                            if (totalMineCount == flagCount)
                            {
                                finishBtn.Enabled = true;
                            }
                            else
                                finishBtn.Enabled = false;

                            return;
                        }

                        m.SetBlock(Controls, block.coordinates, block.tags.Append("Flag").ToList(), TileColors.FlagTile);
                        m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                        flagCount++;
                        UpdateCounters();

                        if (totalMineCount == flagCount)
                        {
                            foreach (var b in m.Blocks)
                            {
                                if (!(bool)b.linkedData[0] && !b.tags.Contains("Flag")) return;
                            }

                            finishBtn.Enabled = true;
                        }
                        else
                            finishBtn.Enabled = false;

                        return;
                    }

                    if (block.tags.Contains("Mine") && !block.tags.Contains("Flag"))
                    {
                        HasWonOrFailed = true;

                        for (int i = 0; i < m.Blocks.Count; i++)
                        {
                            var blockTemp = m.Blocks[i];

                            if (blockTemp.tags.Contains("Mine") && blockTemp.tags.Contains("Flag"))
                            {
                                m.SetBlock(Controls, blockTemp.coordinates, new List<string>() { "CorrectFlag" }, TileColors.CorrFlagTile);
                            }
                            else if (blockTemp.tags.Contains("Mine") && !blockTemp.tags.Contains("Flag"))
                            {
                                m.SetBlock(Controls, blockTemp.coordinates, new List<string>() { "Mine" }, TileColors.MineTile);
                            }
                            else if (!blockTemp.tags.Contains("Mine") && blockTemp.tags.Contains("Flag"))
                            {
                                m.SetBlock(Controls, blockTemp.coordinates, new List<string>() { "WrongFlag" }, TileColors.WrongFlagTile);
                            }
                        }

                        MessageBox.Show("You lost");

                        return;
                    }

                    block.linkedData[0] = true;

                    switch (block.tags[0])
                    {
                        case "None":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.NoneTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            OpenEmptyTiles(m, block);
                            break;
                        case "One":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.OneTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Two":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.TwoTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Three":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.ThreeTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Four":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.FourTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Five":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.FiveTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Six":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.SixTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Seven":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.SevenTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        case "Eight":
                            m.SetBlock(Controls, block.coordinates, block.tags, TileColors.EightTile);
                            m.Blocks[m.Blocks.IndexOf(m.GetBlock(block.coordinates))].linkedData = block.linkedData;
                            break;
                        default:
                            break;
                    }

                    if (totalMineCount == flagCount)
                    {
                        foreach (var b in m.Blocks)
                        {
                            if (!(bool)b.linkedData[0] && !b.tags.Contains("Flag")) return;
                        }

                        finishBtn.Enabled = true;
                    }
                    else
                        finishBtn.Enabled = false;

                };
                UpdateCounters();
            }

            explFlagBtn.Click += (s, e) =>
            {
                IsExploring = !IsExploring;

                explFlagBtn.Text = IsExploring ? "Explore" : "Flag";
            };


            finishBtn.Click += (s, e) =>
            {
                HasWonOrFailed = true;

                bool won = true;

                for (int i = 0; i < m.Blocks.Count; i++)
                {
                    var block = m.Blocks[i];


                    if (block.tags.Contains("Mine") && block.tags.Contains("Flag"))
                    {
                        m.SetBlock(Controls, block.coordinates, new List<string>() { "CorrectFlag" }, TileColors.CorrFlagTile);
                    }
                    else if(block.tags.Contains("Mine") && !block.tags.Contains("Flag"))
                    {
                        m.SetBlock(Controls, block.coordinates, new List<string>() { "Mine" }, TileColors.MineTile);
                        won = false;
                    }
                    else if (!block.tags.Contains("Mine") && block.tags.Contains("Flag"))
                    {
                        m.SetBlock(Controls, block.coordinates, new List<string>() { "WrongFlag" }, TileColors.WrongFlagTile);
                        won = false;
                    }
                }

                string text = won ? "You won!" : "You lost";
                MessageBox.Show(text);
            };
        }

        public void OpenEmptyTiles(Matrix m, Block block)
        {
            var n = m.GetNeighbours(block);

            foreach (var b in n)
            {
                if ((bool)b.linkedData[0]) continue;

                BlockColor fill;

                switch (b.tags[0])
                {
                    case "One":
                        fill = TileColors.OneTile;
                        break;
                    case "Two":
                        fill = TileColors.TwoTile;
                        break;
                    case "Three":
                        fill = TileColors.ThreeTile;
                        break;
                    case "Four":
                        fill = TileColors.FourTile;
                        break;
                    case "Five":
                        fill = TileColors.FiveTile;
                        break;
                    case "Six":
                        fill = TileColors.SixTile;
                        break;
                    case "Seven":
                        fill = TileColors.SevenTile;
                        break;
                    case "Eight":
                        fill = TileColors.EightTile;
                        break;
                    default:
                        fill = TileColors.NoneTile;
                        break;
                }

                m.SetBlock(Controls, b.coordinates, b.tags, fill);
                b.linkedData[0] = true;
                m.Blocks[m.Blocks.IndexOf(m.GetBlock(b.coordinates))].linkedData = b.linkedData;
                if(b.tags.Contains("None"))
                    OpenEmptyTiles(m, b);
            }
        }

        public void UpdateCounters()
        {
            flagsLabel.Text = $"Flags: {flagCount}";
            minesLabel.Text = $"Mines: {totalMineCount}";
        }
    }

    public static class TileColors
    {
        public static BlockColor NoneTile = SystemColors.ControlLightLight;

        public static BlockColor ClosedTile = SystemColors.ControlLight;

        public static BlockColor MineTile = Color.FromArgb(255, 0, 0);

        public static BlockColor FlagTile = Color.FromArgb(255, 255, 0);
        public static BlockColor CorrFlagTile = Color.FromArgb(248, 121, 175);
        public static BlockColor WrongFlagTile = Color.FromArgb(148, 0, 211);

        public static BlockColor OneTile = Color.FromArgb(38, 136, 255);
        public static BlockColor TwoTile = Color.FromArgb(1, 50, 32);
        public static BlockColor ThreeTile = Color.FromArgb(220, 20, 60);
        public static BlockColor FourTile = Color.FromArgb(25, 25, 112);
        public static BlockColor FiveTile = Color.FromArgb(128, 0, 0);
        public static BlockColor SixTile = Color.FromArgb(72, 209, 204);
        public static BlockColor SevenTile = BlockColor.GetColor("_BLACK");
        public static BlockColor EightTile = SystemColors.ControlDark;
    }
}
