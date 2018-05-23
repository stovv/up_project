package com.product.dstavrov.up.Helpers;

import java.util.ArrayList;

/**
 * Created by vicru on 20.03.2018.
 */

public class Group {
    private String name;
    private ArrayList<Day> up_rasp;
    private ArrayList<Day> down_rasp;

    public Group(){
        up_rasp = new ArrayList<Day>();
        down_rasp = new ArrayList<Day>();
    }
    public void setName(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }
    public void up_add(ArrayList<Day> days){
        up_rasp.addAll(days);

    }
    public void down_add(ArrayList<Day> days){
        down_rasp.addAll(days);

    }

    public ArrayList<Day> getDown_rasp() {
        return down_rasp;
    }

    public ArrayList<Day> getUp_rasp() {
        return up_rasp;
    }
}
