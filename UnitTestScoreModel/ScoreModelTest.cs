using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScoreModelImplement;
using System;

namespace UnitTestScoreModel
{
    [TestClass]
    public class ScoreModelTest
    {
        [TestMethod]
        public void Valid_ReturnScore_AllOnes()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            int[] choices = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            double expected = 234;

            // Act
            double actual = testModel.ReturnScore(choices);

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "������ �� ������������ ������ ������");
        }

        [TestMethod]
        public void Valid_ToPolish_AllOperators()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2^3*4-1/2";
            string expected = "1,2,3,^,4,*,1,2,/,-,+";

            // Act
            string actual = testModel.ToReversePolishNotation(mathExpression);

            // Assert
            Assert.AreEqual(expected, actual, "������ ��������");
        }

        [TestMethod]
        public void Valid_ToPolish_DoubleBrackets()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2*(3-4+5*((6+7)))/8";
            string expected = "1,2,3,4,-,5,6,7,+,*,+,*,8,/,+";

            // Act
            string actual = testModel.ToReversePolishNotation(mathExpression);

            // Assert
            Assert.AreEqual(expected, actual, "������ ��������");
        }

        [TestMethod]
        public void Exception_ToPolish_MoreOpenBrackets()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2*(3-4+5*(6+7)()/8";
            string expected = "���������� ����������� ������ ������� ���������� �����������";

            // Act
            try
            {
                // Act
                string actual = testModel.ToReversePolishNotation(mathExpression);
            }
            catch (System.Exception e)
            {
                // Assert
                StringAssert.Contains(e.Message, expected);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Exception_ToPolish_MoreCloseBrackets()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2*(3-4+5*6+7))/8";
            string expected = "���������� ����������� ������ ������� ���������� �����������";

            // Act
            try
            {
                // Act
                string actual = testModel.ToReversePolishNotation(mathExpression);
            }
            catch (System.Exception e)
            {
                // Assert
                StringAssert.Contains(e.Message, expected);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Exception_ToPolish_WrongBrackets()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2*(3-4+5*6)+7)1(3/8";
            string expected = "������ � �������� ���������: �������� ������������������ ������";

            // Act
            try
            {
                // Act
                string actual = testModel.ToReversePolishNotation(mathExpression);
            }
            catch (System.Exception e)
            {
                // Assert
                StringAssert.Contains(e.Message, expected);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }


        [TestMethod]
        public void Exception_ToPolish_DoubleOperator()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2*(3-4+5*-(6+7))/8";
            string expected = "������ � �������� ���������: ��� ��������� ������";

            // Act
            try
            {
                // Act
                string actual = testModel.ToReversePolishNotation(mathExpression);
            }
            catch (System.Exception e)
            {
                // Assert
                StringAssert.Contains(e.Message, expected);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Exception_ToPolish_NotOperator()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
�������� ���������!0,3:�� � �����_0!� �����_+40
����!0,8:���_-10!����_+40
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������������!1:���/�������������_-40!����/�����������_+50
�������� ����!0,4:���_-40!����� 2 ���_0!����� 2 ��� � ����� 5 ���_+20!����� 5 ���_+40
��. �/�!1,2:���� 2x����(24�)_-20!2-4 ���� (24-48�)_+20!���� 4x����(48�)_+40
���������� ������!0,3:�� ���������_-20!���������/��������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������
            ScoreModel testModel = new ScoreModel(data);
            string mathExpression = "1+2*(3-4+5*(6+7_8))/8";
            string expected = "������ � �������� ���������: ����������� ������ �� ���������� �������������� ����������";

            // Act
            try
            {
                // Act
                string actual = testModel.ToReversePolishNotation(mathExpression);
            }
            catch (System.Exception e)
            {
                // Assert
                StringAssert.Contains(e.Message, expected);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void Valid_CalcWithPolish_AllOperators()
        {
            // Arrange
            string data = @"�������!1,5:�� 25 ���_-20!25-50_+40!�� 50_-10
���� �� � ������ ������ ������!0,5:���_+20!����_-10
��������� �������!0,5:�������������_-30!�������������_+20
���������� ������ �� ��������� 5 ���!1,5:���_+30!1-2_0!3-4_-20!����� 4_-40
������������ ����!1,2:�� ����_0!1-2 ����_+20!3-5 ���_+30!����� 5_+40
������� ����������� ������!0,7:���_0!����_+30"; //������ ���������� ������� ��� ������ ��������

            ScoreModel testModel = new ScoreModel(data);
            int[] choices = { 1, 1, 0, 2, 0, 1};
            string mathExpression = "1+2-3*4/6^5";
            double expected = 40*1.5 + -10*0.5 - -30*0.5 * -20*1.5 / Math.Pow(30*0.7, 0*1.2);
            testModel.ToReversePolishNotation(mathExpression);

            // Act
            double actual = testModel.CalculateWithPolish(choices);

            // Assert
            Assert.AreEqual(expected, actual, "������ ����������");
        }


        //TODO: �������� ��� ������ � ���� ��������
        //TODO: �������������� ������ �������
    }
}
