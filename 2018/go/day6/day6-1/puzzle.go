package main

import (
	"fmt"
	"time"

	"github.com/nuclearcookie/aoc2018/day6/input"
)

type Point struct {
	x, y int
}

func main() {
	startTime := time.Now()
	data := input.GetSplit()
	var points []Point
	maxX, maxY := parse(&points, &data)
	grid := *plot(&points, maxX, maxY)
	expand(&grid, &points, maxX, maxY)
	fmt.Println(grid)
	//	fmt.Println(points)
	duration := time.Since(startTime)
	fmt.Printf("Duration: %s\n", duration)
}

func parse(points *[]Point, data *[]string) (int, int) {
	maxX, maxY := 0, 0
	for _, v := range *data {
		point := Point{0, 0}
		fmt.Sscanf(v, "%d, %d", &point.x, &point.y)
		if point.x > maxX {
			maxX = point.x
		}
		if point.y > maxY {
			maxY = point.y
		}
		*points = append(*points, point)
	}
	return maxX + 1, maxY + 1
}

func plot(points *[]Point, maxX, maxY int) *[][]int {
	grid := make([][]int, maxX)
	for i := range grid {
		grid[i] = make([]int, maxY)
	}
	for i, v := range *points {
		grid[v.x][v.y] = i
	}
	return &grid
}

func expand(gridPtr *[][]int, points *[]Point, lengthX, lengthY int) {
	breath := 1
	for {
		if expandWithBreath(gridPtr, points, lengthX, lengthY, breath) {
			breath++
		} else {
			break
		}
	}
	fmt.Println(*gridPtr)
}

func expandWithBreath(gridPtr *[][]int, points *[]Point, lengthX, lengthY, breath int) bool {
	grid := *gridPtr
	grew := false
	for i, v := range *points {
		minX := v.x - breath
		if minX < 0 {
			minX = 0
		}
		maxX := v.x + breath
		if maxX >= lengthX {
			maxX = lengthX - 1
		}
		minY := v.y - breath
		if minY < 0 {
			minY = 0
		}
		maxY := v.y + breath
		if maxY >= lengthY {
			maxY = lengthY - 1
		}
		for x := minX; x <= maxX; x++ {
			for y := minY; y <= maxY; y++ {
				if grid[x][y] == 0 {
					grid[x][y] = i
					grew = true
				}
			}
		}
	}
	return grew
}
