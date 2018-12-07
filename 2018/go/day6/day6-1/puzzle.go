package main

import (
	"fmt"
	"math"
	"time"

	"github.com/nuclearcookie/aoc2018/day6/input"
)

type Point struct {
	X, Y int
}

func (point Point) String() string {
	if point.X > -1 {
		if point.Y == 0 {
			return fmt.Sprintf("%s", string('A'+point.X))
		} else {
			return fmt.Sprintf("%s", string('a'+point.X))
		}
	}
	return "."
}

func main() {
	startTime := time.Now()
	data := input.GetSplit()
	var points []Point
	maxX, maxY := parse(&points, &data)
	grid := *plot(&points, maxX, maxY)
	printGrid(&grid)
	areaPerZone := make(map[int]int)
	for _, row := range grid {
		for _, v := range row {
			areaPerZone[v.X]++
		}
	}
	maxArea := 0
	for _, v := range areaPerZone {
		if v > maxArea {
			maxArea = v
		}
	}
	fmt.Printf("Max area: %d\n", maxArea)
	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
}

func parse(points *[]Point, data *[]string) (int, int) {
	maxX, maxY := 0, 0
	for _, v := range *data {
		point := Point{0, 0}
		fmt.Sscanf(v, "%d, %d", &point.Y, &point.X)
		if point.X > maxX {
			maxX = point.X
		}
		if point.Y > maxY {
			maxY = point.Y
		}
		*points = append(*points, point)
	}
	return maxX + 1, maxY + 1
}

func plot(points *[]Point, maxX, maxY int) *[][]Point {
	grid := make([][]Point, maxX)
	for i := range grid {
		grid[i] = make([]Point, maxY)
		grid[i] = createRow(*points, grid[i], i, maxY)
	}
	return &grid
}

func createRow(points, row []Point, i, maxY int) []Point {
	row = make([]Point, maxY)
	for j := range row {
		row[j] = findShortestManhattanDistance(points, i, j)
	}
	return row
}

func findShortestManhattanDistance(points []Point, x, y int) Point {
	shortest := Point{-1, math.MaxInt32}
	for i, point := range points {
		distanceToPoint := int(math.Abs(float64(point.X-x)) + math.Abs(float64(point.Y-y)))
		if distanceToPoint < shortest.Y {
			shortest.X = i
			shortest.Y = distanceToPoint
		} else if distanceToPoint == shortest.Y {
			shortest.X = -1
			shortest.Y = -1
		}
	}
	return shortest
}

func expand(gridPtr *[][]Point, lengthX, lengthY int) {
	breath := 0
	for {
		if expandWithBreath(gridPtr, lengthX, lengthY, breath) {
			breath++
		} else {
			break
		}
	}
}

func expandWithBreath(gridPtr *[][]Point, lengthX, lengthY, breath int) bool {
	grid := *gridPtr
	grew := false
	checkElem := func(point, ref *Point) {
		if point.X == 0 {
			point.X = ref.X
			point.Y = ref.Y + 1
		} else if point.X != ref.X && point.Y == ref.Y+1 {
			point.X = -1
			point.Y = -1
		} else {
			return
		}
		grew = true
	}
	for i, row := range grid {
		for j, v := range row {
			if v.X != 0 && v.Y == breath {
				if i > 0 {
					elem := &grid[i-1][j]
					checkElem(elem, &v)
				}
				if i < lengthX-1 {
					elem := &grid[i+1][j]
					checkElem(elem, &v)
				}
				if j > 0 {
					elem := &grid[i][j-1]
					checkElem(elem, &v)
				}
				if j < lengthY-1 {
					elem := &grid[i][j+1]
					checkElem(elem, &v)
				}
			}
		}
	}
	return grew
}

func printGrid(grid *[][]Point) {
	for _, row := range *grid {
		fmt.Println(row)
	}
}
