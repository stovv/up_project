package com.product.dstavrov.up.Helpers;

import java.util.HashMap;
import java.util.Map;

/**
 * Created by vicru on 20.03.2018.
 */

public class Day {
    private String name;
    private Map<String, Subject> rasp;
    public Day(){
        rasp = new HashMap<String, Subject>();
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void add_subject(String time, Subject subject){//time format (7_30-9_10)
        rasp.put(time, subject);
    }

    public Map<String, Subject> getRasp() {
        return rasp;
    }
}
