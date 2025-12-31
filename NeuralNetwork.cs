using System;

namespace MediaControlRecognizer
{
    public class NeuralNetwork
    {
        private int inputSize;
        private int hidden1Size;
        private int hidden2Size;
        private int outputSize;

        private double[,] wInputHidden1;
        private double[,] wHidden1Hidden2;
        private double[,] wHidden2Output;

        private double[] bHidden1;
        private double[] bHidden2;
        private double[] bOutput;

        private double learningRate = 0.05; // Уменьшен learning rate для лучшей сходимости
        private Random rnd = new Random();

        public NeuralNetwork(int inputs, int h1, int h2, int outputs)
        {
            inputSize = inputs;
            hidden1Size = h1;
            hidden2Size = h2;
            outputSize = outputs;

            wInputHidden1 = InitMatrix(inputSize, hidden1Size);
            wHidden1Hidden2 = InitMatrix(hidden1Size, hidden2Size);
            wHidden2Output = InitMatrix(hidden2Size, outputSize);

            bHidden1 = new double[hidden1Size];
            bHidden2 = new double[hidden2Size];
            bOutput = new double[outputSize];
        }

        private double[,] InitMatrix(int rows, int cols)
        {
            double[,] matrix = new double[rows, cols];
            double range = Math.Sqrt(2.0 / rows); // Инициализация He
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = (rnd.NextDouble() * 2 - 1) * range;
            return matrix;
        }

        private double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));
        private double SigmoidDerivative(double y) => y * (1.0 - y);

        public double[] FeedForward(double[] inputs)
        {
            var h1 = ComputeLayer(inputs, wInputHidden1, bHidden1);
            var h2 = ComputeLayer(h1, wHidden1Hidden2, bHidden2);
            var outLayer = ComputeLayer(h2, wHidden2Output, bOutput);
            return outLayer;
        }

        public void Train(double[] inputs, double[] targets)
        {
            var h1 = ComputeLayer(inputs, wInputHidden1, bHidden1);
            var h2 = ComputeLayer(h1, wHidden1Hidden2, bHidden2);
            var outputs = ComputeLayer(h2, wHidden2Output, bOutput);

            // Обратное распространение ошибки
            double[] outGradients = new double[outputSize];
            for (int i = 0; i < outputSize; i++)
            {
                double error = targets[i] - outputs[i];
                outGradients[i] = error * SigmoidDerivative(outputs[i]);
            }

            double[] h2Gradients = new double[hidden2Size];
            for (int i = 0; i < hidden2Size; i++)
            {
                double errorSum = 0;
                for (int j = 0; j < outputSize; j++)
                    errorSum += outGradients[j] * wHidden2Output[i, j];
                h2Gradients[i] = errorSum * SigmoidDerivative(h2[i]);
            }

            double[] h1Gradients = new double[hidden1Size];
            for (int i = 0; i < hidden1Size; i++)
            {
                double errorSum = 0;
                for (int j = 0; j < hidden2Size; j++)
                    errorSum += h2Gradients[j] * wHidden1Hidden2[i, j];
                h1Gradients[i] = errorSum * SigmoidDerivative(h1[i]);
            }

            // Обновление весов
            UpdateWeights(wHidden2Output, bOutput, h2, outGradients);
            UpdateWeights(wHidden1Hidden2, bHidden2, h1, h2Gradients);
            UpdateWeights(wInputHidden1, bHidden1, inputs, h1Gradients);
        }

        private double[] ComputeLayer(double[] input, double[,] weights, double[] biases)
        {
            int outLen = weights.GetLength(1);
            int inLen = weights.GetLength(0);
            double[] res = new double[outLen];

            for (int j = 0; j < outLen; j++)
            {
                double sum = 0;
                for (int i = 0; i < inLen; i++)
                    sum += input[i] * weights[i, j];
                sum += biases[j];
                res[j] = Sigmoid(sum);
            }
            return res;
        }

        private void UpdateWeights(double[,] weights, double[] biases, double[] inputs, double[] gradients)
        {
            int rows = weights.GetLength(0);
            int cols = weights.GetLength(1);

            for (int j = 0; j < cols; j++)
            {
                biases[j] += gradients[j] * learningRate;
                for (int i = 0; i < rows; i++)
                {
                    weights[i, j] += inputs[i] * gradients[j] * learningRate;
                }
            }
        }
    }
}