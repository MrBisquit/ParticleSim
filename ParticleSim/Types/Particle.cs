using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSim.Types
{
    internal class Particle
    {
        public int temperature { get; set; } = 0; // In degrees centigrade
        public int speed { get; set; } = 0; // Depending on the temp

        /* Rates of speed:
         * Freezing = 0 movements per 10 ticks (Unless Gas)
         * Cold = 0-5 movements per 10 ticks
         * Normal = 5-10 movements per 10 ticks
         * Warm = 10-15 movements per 10 ticks
         */

        public ParticleType type { get; set; } = ParticleType.Gas; // Gas as just the default, this wont matter as it'll be set when the class is initialised.

        // It's location
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;

        // Specific particle location
        public int px { get; set; } = 0;
        public int py { get; set; } = 0;

        public Particle(int temperature, int speed, ParticleType type, int x, int y, int px, int py)
        {
            this.temperature = temperature;
            this.speed = speed;
            this.type = type;
            this.x = x;
            this.y = y;
            this.px = px;
            this.py = py;
        }

        public Bitmap Tick(int tick, Bitmap BM)
        {
            Color color = Color.White;

            if(type == ParticleType.Gas)
            {
                color = Color.Orange; // Because why not
                color = Color.FromArgb(255 / (temperature + 1), color.R, color.G, color.B); // Trust me, I have no clue what I'm doing either
            }

            BM.SetPixel(x, y, color);

            return BM;
        }
    }

    internal enum ParticleType
    {
        Gas = 0,       // Moves freely
        Liquid = 1,    // Is a puddle, because water
        Solid = 2,     // Is block
        Ice_cream = 3, // Felt like it
        Plasma = -1    // No
    }
}
