using System;
using VRageMath;

namespace DiamondDomeDefense
{

    public class PredictiveController
    {

        const int MIN_REFRESH_INTERVAL = 3;

        double gain_output;
        double gain_predict;
        double output_limit;

        double predictedDelta;

        double prevExpected;
        int prevClock;
        public PredictiveController(double outputGain, double predictiveGain, double outputLimit)
        { 
            gain_output = outputGain;
            gain_predict = predictiveGain;
            output_limit = outputLimit;
        }
        public double Filter(double current, double expected, int clock)
        {
            int deltaSteps = Math.Max(clock - prevClock, 1);

            double currentDelta = expected - prevExpected;

            if (deltaSteps < MIN_REFRESH_INTERVAL)
            {
                    currentDelta *= (double)MIN_REFRESH_INTERVAL / deltaSteps;
                    deltaSteps = MIN_REFRESH_INTERVAL;
            }

            AdjustAnglePI(ref currentDelta);

            if (predictedDelta * currentDelta < 0)
            {
                predictedDelta = (gain_predict * currentDelta); 
            } 
            else 
            { 
                predictedDelta = ((1 - gain_predict) * predictedDelta) + (gain_predict * currentDelta); 
            }

            double delta = expected - current + predictedDelta;
            AdjustAnglePI(ref delta);

            prevExpected = expected;
            prevClock = Math.Max(clock, prevClock);

            return MathHelper.Clamp(delta * gain_output / deltaSteps, -output_limit, output_limit);
        }
            public void AdjustAnglePI(ref double value)
            {
                if (value < -Math.PI) 
            { 
                value += MathHelperD.TwoPi;
                if (value < -Math.PI) value += MathHelperD.TwoPi; 
            }
                else if (value > Math.PI) 
            {
                value -= MathHelperD.TwoPi;
                if (value > Math.PI) value -= MathHelperD.TwoPi;
            }
            }
            public void Reset() 
            { 
            prevClock = 0;
            predictedDelta = prevExpected = 0; 
            }
    }
    
}
