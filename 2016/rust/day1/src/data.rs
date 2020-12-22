use coordinate::Coordinate;
use std::collections::btree_set::BTreeSet;

enum Direction {
    North,
    South,
    East,
    West
}

pub struct Data {
    current_direction : Direction,
    pub total_coordinates : Coordinate,
    current_coordinate : Coordinate,
    visited_locations : BTreeSet<Coordinate>
}

impl Data {
    pub fn new() -> Data {
        Data {
            current_direction : Direction::North,
            total_coordinates : Coordinate::new(),
            current_coordinate : Coordinate::new(),
            visited_locations : BTreeSet::new()
        }
    }
    pub fn update_current_direction( &mut self, new_direction : char ) {

        if new_direction == 'R' {
            self.current_direction = match self.current_direction {
                Direction::North => Direction::East,
                Direction::East => Direction::South,
                Direction::South => Direction::West,
                Direction::West => Direction::North
            }
        } else if new_direction == 'L' {
            self.current_direction = match self.current_direction {
                Direction::North => Direction::West,
                Direction::East => Direction::North,
                Direction::South => Direction::East,
                Direction::West => Direction::South
            }
        } else {
            panic!("Unknown direction!");
        }
    }

    pub fn walk( &mut self, distance : u32 ) {
        let distance = distance as i32;
        let previous_coords = self.current_coordinate.clone();
        for _ in 0..distance {
            match self.current_direction {
                Direction::North => {
                        self.current_coordinate.vertical_movement += 1;
                },
                Direction::South => {
                        self.current_coordinate.vertical_movement -= 1;
                },
                Direction::East => {
                        self.current_coordinate.horizontal_movement += 1;
                },
                Direction::West => {
                        self.current_coordinate.horizontal_movement -= 1;
                },
            }
            if !self.visited_locations.insert(self.current_coordinate.clone()) {
                println!("Intersection found at {} blocks away", Data::get_taxicab_coord_destination(&self.current_coordinate));
            }
        }
        // :NOTE: Why do I have to do this?
        let current_coordinate_copy = self.current_coordinate.clone();
        // :NOTE: --end
        self.total_coordinates += current_coordinate_copy - previous_coords;
    }

    pub fn get_taxicab_coord_destination(coordinate: & Coordinate) -> i32 {
        coordinate.horizontal_movement.abs() + coordinate.vertical_movement.abs()
    }
}
