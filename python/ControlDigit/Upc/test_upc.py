import unittest

from upc import upc


class TestUpc(unittest.TestCase):
    def test_upc(self):
        self.assertEqual(upc(0), 0)
        self.assertEqual(upc(1), 7)
        self.assertEqual(upc(2), 4)
        self.assertEqual(upc(9), 3)
        self.assertEqual(upc(10), 9)
        self.assertEqual(upc(13), 0)
        self.assertEqual(upc(15), 4)
        self.assertEqual(upc(17), 8)
        self.assertEqual(upc(18), 5)
        self.assertEqual(upc(11111111111), 7)
        self.assertEqual(upc(12345678901), 2)
        self.assertEqual(upc(98765432101), 2)
        self.assertEqual(upc(11223344556), 2)
        self.assertEqual(upc(32512312431), 1)
        self.assertEqual(upc(98439874398), 8)
        self.assertEqual(upc(98439876398), 6)


if __name__ == "__main__":
    unittest.main()
