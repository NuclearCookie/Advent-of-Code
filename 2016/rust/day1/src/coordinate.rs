use std::ops::*;
use std::cmp::Ordering;

#[derive(Clone, Eq)]
pub struct Coordinate {
    pub horizontal_movement : i32,
    pub vertical_movement : i32
}

impl Coordinate {
    pub fn new() -> Coordinate {
        Coordinate { horizontal_movement : 0, vertical_movement : 0 }
    }
}

impl Add for Coordinate {
    type Output = Coordinate;

    fn add(self, other: Coordinate) -> Coordinate {
        Coordinate {
            horizontal_movement: self.horizontal_movement + other.horizontal_movement,
            vertical_movement: self.vertical_movement + other.vertical_movement
        }
    }
}

impl AddAssign for Coordinate {
    fn add_assign(&mut self, other: Coordinate) {
            self.horizontal_movement += other.horizontal_movement;
            self.vertical_movement += other.vertical_movement;
    }
}

impl Sub for Coordinate {
    type Output = Coordinate;

    fn sub(self, other: Coordinate) -> Coordinate {
        Coordinate {
            horizontal_movement: self.horizontal_movement - other.horizontal_movement,
            vertical_movement: self.vertical_movement - other.vertical_movement
        }
    }
}

impl SubAssign for Coordinate {
    fn sub_assign(&mut self, other: Coordinate) {
            self.horizontal_movement -= other.horizontal_movement;
            self.vertical_movement -= other.vertical_movement;
    }
}

impl Ord for Coordinate {
    fn cmp(&self, other: &Coordinate) -> Ordering {
        (self.horizontal_movement, self.vertical_movement).cmp(&(other.horizontal_movement, other.vertical_movement))
    }
}

impl PartialOrd for Coordinate {
    fn partial_cmp(&self, other: &Coordinate) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl PartialEq for Coordinate {
    fn eq(&self, other: &Coordinate) -> bool {
        (self.horizontal_movement, self.vertical_movement) == (other.horizontal_movement, other.vertical_movement)
    }
}
