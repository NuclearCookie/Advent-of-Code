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
	infiniteZones := getInfiniteZones(&grid)
	// printGrid(&grid)
	areaPerZone := make(map[int]int)
	for _, row := range grid {
		for _, v := range row {
			_, exists := infiniteZones[v.X]
			if !exists {
				areaPerZone[v.X]++
			}
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
			shortest.Y = distanceToPoint
		}
	}
	return shortest
}

func getInfiniteZones(gridPtr *[][]Point) map[int]bool {
	grid := *gridPtr
	top := grid[0]
	bottom := grid[len(grid)-1]
	infiniteValues := make(map[int]bool)
	for _, v := range grid {
		infiniteValues[v[0].X] = true
		infiniteValues[v[len(v)-1].X] = true
	}
	for _, v := range top {
		infiniteValues[v.X] = true
	}
	for _, v := range bottom {
		infiniteValues[v.X] = true
	}
	return infiniteValues
}

func printGrid(grid *[][]Point) {
	for _, row := range *grid {
		fmt.Println(row)
	}
}
