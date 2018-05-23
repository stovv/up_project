package com.product.dstavrov.up.Item;


import com.product.dstavrov.up.Helpers.Subject;

/**
 * Created by vicru on 26.03.2018.
 */

public class SubjectItem {

    private String time;
    private Subject subject;
    private int color;

    public SubjectItem(String time, Subject subject, int color) {
        this.time = time;
        this.subject = subject;
        this.color = color;
    }

    public String getTime() {
        return time;
    }

    public String getAud_num() {
        return subject.getAud();
    }

    public String getName_of_prof() {
        return subject.getLector();
    }

    public String getName_of_subject() {
        return subject.getName();
    }

    public int getColor(){
        return color;
    }
}
