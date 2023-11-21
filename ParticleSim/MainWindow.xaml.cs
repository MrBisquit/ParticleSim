using Ookii.Dialogs.Wpf;
using ParticleSim.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParticleSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Bitmap MainBM = new Bitmap(1600, 900); // 16:9 ratio
        Bitmap PBM = new Bitmap(4, 4); // 4x4 because yes

        List<Particle> Particles = new List<Particle>();

        int CurrentTick = 0; // Keeps on going up

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TaskDialog td = new TaskDialog()
            {
                WindowTitle = "ParticleSim",
                Content = "Press Run to start the simulation! And click it again to pause/stop it.\nBy default hovering/clicking on a particle will enlarge it on the right.\n" +
                "To spawn particles:\n" +
                "- At the top, click Particles\n" +
                "- Click Spawn Particles\n" +
                "- Click on the particle type you want"
            };

            TaskDialogButton tdb = new TaskDialogButton()
            {
                Text = "Close the main window to close this",
                Enabled = false, // L
            };

            td.Buttons.Add(tdb);

            Task.Factory.StartNew(() => { td.Show(); }); // New thread so it doesen't freeze the current one

            SpawnParticles(300, ParticleType.Gas); // Spawn some basic

            // Clearing the Bitmaps
            // Not the most efficient way but it does what it needs to
            for (int i = 0; i < MainBM.Width; i++)
            {
                for (int j = 0; j < MainBM.Height; j++)
                {
                    MainBM.SetPixel(i, j, System.Drawing.Color.Black);
                }
            }

            for (int i = 0; i < PBM.Width; i++)
            {
                for (int j = 0; j < PBM.Height; j++)
                {
                    PBM.SetPixel(i, j, System.Drawing.Color.Black);
                }
            }

            UpdateImages(); // Update the images
        }

        private void UpdateImages()
        {
            Main.Source = ConvertToBitmapImage(MainBM);  // Set the big image
            Particle.Source = ConvertToBitmapImage(PBM); // Set the smaller image next to the big image

            ParticlesCount.Content = $"Particles: {Particles.Count}";
        }

        // Bing AI, I know how it works (I don't)
        public BitmapImage ConvertToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void SpawnParticles(int Amount, ParticleType ParticleType, bool KeepExisting = false)
        {
            if(!KeepExisting) Particles.Clear();

            for (int i = 0; i < Amount; i++)
            {
                Particles.Add(new Particle(0, 0, ParticleType, 0, 0, 0, 0));
            }

            Tick();
            UpdateImages();
        }

        private void Tick()
        {
            CurrentTick++;

            Particles.ForEach(Particle =>
            {
                MainBM = Particle.Tick(CurrentTick % 10, MainBM); // RIP your computer
            });
        }
    }
}
