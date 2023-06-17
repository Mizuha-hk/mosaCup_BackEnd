using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyLevelMLModel
{
    public static class UseJoyModel
    {
        public static int JudgeJoyLevel(string Text)
        {
            var Data = new JoyLevelModel.ModelInput();
            Data.Sentence = Text;
            var result = JoyLevelModel.Predict(Data);

            return Convert.ToInt32(result.Avg__Readers_Joy);
        }
    }
}
