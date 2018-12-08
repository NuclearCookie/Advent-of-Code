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

var maxDistanceToAllPoints = 10000

func (point Point) String() string {
	if point.Y < 32 {
		return fmt.Sprint("#")
	}
	return "."
}

func main() {
	startTime := time.Now()
	data := input.GetSplit()
	var points []Point
	maxX, maxY := parse(&points, &data)
	grid := *plot(&points, maxX, maxY)
	// printGrid(&grid)
	totalPointsWithinLimit := 0
	for _, row := range grid {
		for _, v := range row {
			if v.Y < maxDistanceToAllPoints {
				totalPointsWithinLimit++
			}
		}
	}
	fmt.Printf("Total points within limit: %d\n", totalPointsWithinLimit)
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
		row[j] = findTotalManhattanDistance(points, i, j)
	}
	return row
}

func findTotalManhattanDistance(points []Point, x, y int) Point {
	summedPoint := Point{-1, 0}
	for _, point := range points {
		distanceToPoint := int(math.Abs(float64(point.X-x)) + math.Abs(float64(point.Y-y)))
		summedPoint.Y += distanceToPoint
	}
	return summedPoint
}

func printGrid(grid *[][]Point) {
	for _, row := range *grid {
		fmt.Println(row)
	}
}
