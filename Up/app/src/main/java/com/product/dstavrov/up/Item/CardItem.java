package com.product.dstavrov.up.Item;


import java.util.List;

public class CardItem {

    private String name_day;
    private List<SubjectItem> subjectItems;

    public CardItem(String name_day, List<SubjectItem> subjectItems) {
        this.name_day = name_day;
        this.subjectItems = subjectItems;
    }

    public String getName_day() {
        return name_day;
    }

    public List<SubjectItem> getSubjectItems() {
        return subjectItems;
    }
}
