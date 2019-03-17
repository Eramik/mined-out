using System;

namespace Mined_Out {
    public static class FieldGenerator {
        public static Field Generate() {
            Field field = PreGenerateField(7, 7);

            return field;
        }

        private static Field PreGenerateField(int height, int width) {
            if (width % 2 == 0) {
                throw new Exception("Width of the game field has to be odd");
            }
            Field field = new Field(height, width);
            int mid = (int)(width / 2);
            // Lower edge
            for(int i = 0; i < width; i++) {
                if (i == mid) {
                    field.SetPlayer(height - 1, mid);
                } else {
                    field[height - 1, i] = new Wall();
                }
            }

            // Left and right edge
            for(int i = 0; i < height - 1; i++) {
                field[i, 0] = new Wall();
                field[i, height - 1] = new Wall();
            }

            return field;
        }
    }
}