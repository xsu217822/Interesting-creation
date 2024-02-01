namespace Electronic_wooden_fish
{
    public partial class Form1 : Form
    {
        private int countFo;
        private int countDao;
        private bool checking = false;
        private int fo = 0;
        private int dao = 0;
        private Level level = new Level();
        private int levelFo = 1;
        private int levelDao = 1;
        Random random = new Random();


        public Form1()
        {
            InitializeComponent();
            textBox3.Text = level.LevelFo[levelFo];
            textBox4.Text = level.LevelDao[levelDao];
            countFo = 0;
            countDao = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = false;
            timer1.Start();
            checking = true;
            button1.Enabled = false;
            button2.Enabled = true;
            progressBar1.Value = fo;
            progressBar1.Maximum = 10 * (int)Math.Pow(10, levelFo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = true;
            timer1.Start();
            checking = false;
            button1.Enabled = true;
            button2.Enabled = false;
            progressBar1.Value = dao;
            progressBar1.Maximum = 10 * (int)Math.Pow(10, levelDao);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checking == true)
            {
                countFo += 1 * levelFo;
                progressBar1.Value += 1 * levelFo;
                fo = progressBar1.Value;
                textBox1.Text = countFo.ToString();
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    progressBar1.Value = 0;
                    levelFo++;
                    progressBar1.Maximum = 10 * (int)Math.Pow(10, levelFo);
                    textBox3.Text = level.LevelFo[levelFo];
                }
            }
            else
            {
                countDao += 1 * levelDao;
                progressBar1.Value += 1 * levelDao;
                dao = progressBar1.Value;
                textBox2.Text = countDao.ToString();
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    progressBar1.Value = 0;
                    levelDao++;
                    progressBar1.Maximum = 10 * (int)Math.Pow(10, levelDao);
                    textBox4.Text = level.LevelDao[levelDao];
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum - 5 * levelFo)
            {
                countFo += 5 * levelFo;
                progressBar1.Value += 5 * levelFo;
            }
            button3.Text = level.stringsFo[random.Next(level.stringsFo.Count)];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum - 5 * levelDao)
            {
                countDao += 5 * levelDao;
                progressBar1.Value += 5 * levelDao;
            }
            button4.Text = level.stringsDao[random.Next(level.stringsDao.Count)];
        }
    }
}
