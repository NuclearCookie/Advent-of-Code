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

func main() {
	start_time := time.Now()
	data := input.GetSplit()
	cut_info := make([]CutInfo, len(data))
	r := regexp.MustCompile("#([0-9]+) @ ([0-9]+),([0-9]+): ([0-9]+)x([0-9]+)")
	for i, v := range data {
		matches := r.FindStringSubmatch(v)
		// :TODO: regexp parse into CutInfo
		id, _ := strconv.Atoi(matches[1])
		left, _ := strconv.Atoi(matches[2])
		top, _ := strconv.Atoi(matches[3])
		width, _ := strconv.Atoi(matches[4])
		height, _ := strconv.Atoi(matches[5])
		cut_info[i].Set(id, left, top, width, height)
	}
	fmt.Println(cut_info)

	duration := time.Since(start_time)
	fmt.Printf("Duration: %s\n", duration)
}
