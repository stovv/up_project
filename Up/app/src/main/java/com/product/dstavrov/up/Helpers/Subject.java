package com.product.dstavrov.up.Helpers;

/**
 * Created by vicru on 20.03.2018.
 */

public class Subject {
    private String name;
    private String aud;
    private String lector;
    public Subject(){}
    public Subject(String name, String aud, String lector){
        this.name = name;
        this.aud = aud;
        this.lector = lector;
    }
    public String getAud() {
        return aud;
    }

    public String getName() {
        return name;
    }

    public String getLector() {
        return lector;
    }

    public void setAud(String aud) {
        this.aud = aud;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setLector(String lector) {
        this.lector = lector;
    }

}
