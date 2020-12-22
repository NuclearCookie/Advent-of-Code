package main

import (
	"fmt"
	"regexp"
	"strconv"
	"time"

	"github.com/nuclearcookie/aoc2018/day3/input"
)

type CutInfo struct {
	id, left, top, width, height int
}

func (this *CutInfo) Set(id, left, top, width, height int) {
	this.id = id
	this.left = left
	this.top = top
	this.width = width
	this.height = height
}

var squareInchInfo [1000][1000]int
var nr_overlapping_squares int

func main() {
	start_time := time.Now()
	data := input.GetSplit()
	cut_infos := make([]CutInfo, len(data))
	c := make(chan CutInfo, len(data))
	go Parse(data, c)

	i := 0
	for cut_info := range c {
		cut_infos[i] = cut_info
		i++
		FillSquareInchInfo(&cut_info)
	}

	duration := time.Since(start_time)
	fmt.Printf("Duration: %s\n", duration)
	fmt.Printf("Overlapping inches: %d", nr_overlapping_squares)
}

func Parse(data []string, c chan CutInfo) {
	r := regexp.MustCompile("#([0-9]+) @ ([0-9]+),([0-9]+): ([0-9]+)x([0-9]+)")
	for _, v := range data {
		matches := r.FindStringSubmatch(v)
		// :TODO: regexp parse into CutInfo
		id, _ := strconv.Atoi(matches[1])
		left, _ := strconv.Atoi(matches[2])
		top, _ := strconv.Atoi(matches[3])
		width, _ := strconv.Atoi(matches[4])
		height, _ := strconv.Atoi(matches[5])
		c <- CutInfo{id, left, top, width, height}
	}
	close(c)
}

func FillSquareInchInfo(info *CutInfo) {
	for x := info.left; x < info.left+info.width; x++ {
		for y := info.top; y < info.top+info.height; y++ {
			squareInchInfo[x][y]++
			if squareInchInfo[x][y] == 2 {
				nr_overlapping_squares++
			}
		}
	}
}
