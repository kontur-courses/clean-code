import unittest

from snils import snils


class TestSnils(unittest.TestCase):
    def test_snils(self):
        self.assertEqual(snils(1), 1)
        self.assertEqual(snils(10), 2)
        self.assertEqual(snils(100), 3)
        self.assertEqual(snils(1001), 4 + 1)
        self.assertEqual(snils(1111), 4 + 3 + 2 + 1)
        self.assertEqual(snils(112233445), 95)
        self.assertEqual(snils(87654303), 0)
        self.assertEqual(snils(87654302), 0)
        self.assertEqual(snils(116973385), 89)
        self.assertEqual(snils(152675138), 70)
        self.assertEqual(snils(463436384), 96)
        self.assertEqual(snils(158757369), 28)
        self.assertEqual(snils(192168000), 62)


if __name__ == "__main__":
    unittest.main()
