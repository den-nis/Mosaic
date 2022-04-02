# Mosaic2

Small CLI and GUI application for creating mosaic images.

## Example output
![Example](https://user-images.githubusercontent.com/50838791/136795543-5277892d-7160-4361-8905-e3f49670081a.jpg)
(Image source https://www.kaggle.com/jessicali9530/stanford-dogs-dataset)

## CLI
```
  -o, --output           (Default: result.png) Output path

  -i, --input            Required. Path for input images

  -m, --main             Required. Main image

  -a, --all              (Default: true) Enable subdirectory search

  -f, --filter           (Default: *.*) The search string to match against the names of files in the input path

  -s, --size             (Default: 0.1) Non zero multiplier for the grid size. A smaller value will result in fewer tiles

  -R, --rotate           (Default: false) Enable tile rotation

  -M, --mirror           (Default: false) Enable tile mirroring

  -S, --samples          (Default: 4) Amount of color samples per tile

  --resolution           (Default: 45) The width and height of the tiles

  --useAverageSamples    (Default: true) Take the average of the area around the sample

  --repeatRadius         (Default: -1) The target minimum distance between 2 repeating tiles (-1 for auto)

  --useGridSearch        (Default: false) Use the grid instead of the lookup when searching for repeating tiles

  --cropMode             (Default: Center) Method for cropping the tile images if they are not square. (Center, Stretch)

  --help                 Display this help screen.

  --version              Display version information.
```


